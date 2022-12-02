using AutoMapper;
using Domain.Users;
using Domain.PricePlans;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Extensions;
using Services.Cryptography;
using Services.Auth;
using Services.Mails;
using Services.PricePlans;
using Squads.Shared.Users;

namespace Services.Users;

public class UserService : IUserService
{
    private readonly SquadContext DbCtx;
    private readonly DbSet<User> users;
    private readonly IMapper mapper;
    private readonly TokenService _tokenService;
    private readonly MailService _mailService;
    private readonly PricePlanService _pricePlanService;

    public UserService(SquadContext dbCtx, TokenService tokenService, MailService mailService, PricePlanService priceService, IMapper mapper)
    {
        DbCtx = dbCtx;
        users = dbCtx.Users;
        this.mapper = mapper;
        _tokenService = tokenService;
        _mailService = mailService;
        _pricePlanService = priceService;
    }

    private IQueryable<User> GetUserById(int id) => users
         .AsNoTracking()
         .Include(u => u.ActivePricePlan)
         .Where(u => u.Id == id);

    public User getUserByIdOrThrow(int id)
    {
        User? user = GetUserById(id).SingleOrDefault();
        if(user == null)
            throw new EntityNotFoundException(nameof(User), id);
        
        return user;
    }

    private IQueryable<User> GetUserByEmail(string email) => users
         .AsNoTracking()
         .Include(u => u.ActivePricePlan)
         .Where(u => u.Email == email);

    public async Task<User> getUserByEmail(string email)
    {
        return await GetUserByEmail(email).SingleAsync();
    }

    public async Task<UserResponse.Create> CreateAsync(UserRequest.Create request)
    {
        UserResponse.Create response = new();
        CryptoService.CreatePasswordHash(
            request.user.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt
        );

        var newUser = mapper.Map<User>(request.user);
        newUser.PasswordHash = passwordHash;
        newUser.PasswordSalt = passwordSalt;
        var user = users.Add(newUser);
        await DbCtx.SaveChangesAsync();
        response.UserId = user.Entity.Id;
        return response;
    }

    public async Task DeleteAsync(UserRequest.Delete request)
    {
        users.RemoveIf(p => p.Id == request.UserId);
        await DbCtx.SaveChangesAsync();
    }

    public async Task<UserResponse.Edit> EditAsync(UserRequest.Edit request)
    {
        UserResponse.Edit response = new();
        var user = await GetUserById(request.UserId).SingleOrDefaultAsync();

        if (user is null) return response;

        var model = request.User;
        user.LastName = model.LastName ?? user.LastName;
        user.FirstName = model.FirstName ?? user.FirstName;
        user.Email = model.Email ?? user.Email;
        user.DateOfBirth = model.DateOfBirth;
        user.Address = model.Address;
        user.PhoneNumber = model.PhoneNumber;   

        DbCtx.Entry(user).State = EntityState.Modified;
        await DbCtx.SaveChangesAsync();
        response.UserId = user.Id;
        return response;
    }

    public async Task<UserResponse.GetDetail> GetDetailAsync(UserRequest.GetDetail request)
    {
        UserResponse.GetDetail response = new();
        var user = await GetUserById(request.UserId)
            .Select(x => mapper.Map<UserDto.Detail>(x))
            .SingleOrDefaultAsync();
        
        if (user == null) return response;
        response.User = user;
        return response;
    }

    public async Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex request)
    {
        UserResponse.GetIndex response = new();
        var query = users.AsQueryable().Include(u => u.ActivePricePlan).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(x => x.Email.Contains(request.SearchTerm));

        response.TotalAmount = query.Count();

        if (request.Amount != null && request.Page != null)
        {
            query = query.Take(request.Amount ?? 0);
            query = query.Skip(request.Amount * request.Page ?? 0);
        }

        query.OrderBy(x => x.LastName);
        response.Users = await query.Select(x => mapper.Map<UserDto.Index>(x)).ToListAsync();

        return response;
    }
    public async Task<UserResponse.GetDetail> GetDetailByEmailAsync(UserRequest.GetDetail request)
    {
        UserResponse.GetDetail response = new();
        var user = await GetUserByEmail(request.Email)
            .Select(x => mapper.Map<UserDto.Detail>(x))
            .SingleOrDefaultAsync();

        if (user == null) return response;
        response.User = user;
        return response;
    }

    public async Task<UserResponse.GetDetail> UpdatePricePlan(int userId, int pricePlanId)
    {   
        User? user = await GetUserById(userId).SingleAsync();
        PricePlan? pricePlan = await DbCtx.PricePlans.Where(p => p.Id == pricePlanId).SingleAsync();
        if(user == null)
        {
            throw new EntityNotFoundException(nameof(User), userId);
        } 
        else if(pricePlan == null)
        {
            throw new EntityNotFoundException(nameof(PricePlan), pricePlanId);
        }

        user.ActivePricePlan = pricePlan;
        DbCtx.Entry(user).State = EntityState.Modified;
        DbCtx.SaveChanges();
        UserResponse.GetDetail response = new();
        response.User = mapper.Map<UserDto.Detail>(user);
        return response;
    }

    public async Task<bool> InviteUser(UserRequest.InviteUser request)
    {
        User newUser = mapper.Map<UserDto.Create, User>(request.User);
        newUser.Status = UserStatus.Inactive;
        CryptoService.CreatePasswordHash(
            "dummy",
            out byte[] passwordHash,
            out byte[] passwordSalt
        );
        newUser.PasswordHash = passwordHash;
        newUser.PasswordSalt = passwordSalt;
        newUser.ActivePricePlan = _pricePlanService.GetDefaultPricePlan();
        User createdUser = users.Add(newUser).Entity;
        await DbCtx.SaveChangesAsync();
        
        string link = CreateActivationLink(createdUser);
        _mailService.sendActivationEmail(createdUser, link);
        return true;
    }

    public async Task<bool> ActivateUser(string token, UserRequest.ActivateUser request)
    {
        var userId = _tokenService.ParseActivationToken(token);   
        User user = getUserByIdOrThrow(userId);
        user.Status = UserStatus.Active;

        CryptoService.CreatePasswordHash(
            request.User.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt
        );

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        if(request.User.DateOfBirth != null) user.DateOfBirth = (DateTime) request.User.DateOfBirth;
        if(request.User.Address != null) user.Address = request.User.Address;
        if(request.User.PhoneNumber != null) user.PhoneNumber = request.User.PhoneNumber;
        DbCtx.Entry(user).State = EntityState.Modified;
        await DbCtx.SaveChangesAsync();
        return true;
    }

    private string CreateActivationLink(User user)
    {
        string host = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";")[0];
        string jwt = _tokenService.CreateActivationToken(user);
        return $"{host}/activate?token={jwt}";
    }
}
