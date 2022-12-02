using Ardalis.GuardClauses;
using AutoMapper;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Services.Auth;
using Services.Cryptography;
using Squads.Shared.Auth;
using Squads.Shared.Users;
using System.Security.Claims;

namespace Services.Sessions;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor HttpContextAccessor;
    private readonly TokenService TokenService;
    private readonly SquadContext DbCtx;
    private readonly DbSet<User> Users;
    private readonly IMapper mapper;

    public AuthService(
        SquadContext dbCtx, 
        TokenService tokenService, 
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
    )
    {
        HttpContextAccessor = httpContextAccessor;
        TokenService = tokenService;
        DbCtx = dbCtx;
        Users = dbCtx.Users;
        this.mapper = mapper;
    }

    private IQueryable<User> GetUserByEmail(string email) => Users
         .AsNoTracking()
         .Where(u => u.Email == email);

    public async Task<AuthResponse.Current> CurrentAsync()
    {
        var email = string.Empty;
        AuthResponse.Current res = new();
        
        if (HttpContextAccessor.HttpContext == null)
        {
            throw new ApplicationException("no http context");
        }
        
        email = HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var user = GetUserByEmail(email).FirstOrDefault();

        Guard.Against.Null(user);

        res.User = mapper.Map<UserDto.Detail>(user);
        
        return res;
    }

    public async Task<AuthResponse.Login> LoginAsync(AuthRequest.Login req)
    {
        var res = new AuthResponse.Login();

        var user = GetUserByEmail(req.User.Email).FirstOrDefault();
        Guard.Against.Null(user);

        if (!CryptoService.VerifyPasswordHash(req.User.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new ApplicationException("Invalid credentials");
        }

        string token = TokenService.CreateToken(user, user.Role);
        res.Token = token;
        res.User = mapper.Map<UserDto.Index>(user);

        return res;
    }

    public async Task<AuthResponse.Register> RegisterAsync(AuthRequest.Register req)
    {
        CryptoService.CreatePasswordHash(
            req.User.Password,
            out byte[] passwordHash,
            out byte[] passwordSalt
       );

        var user = mapper.Map<User>(req.User);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        var addedUser = Users.Add(user).Entity;

        var res = new AuthResponse.Register();
        res.User = mapper.Map<UserDto.Detail>(addedUser);

        await DbCtx.SaveChangesAsync();

        res.Token = TokenService.CreateToken(addedUser, addedUser.Role);

        return res;
    }

}
