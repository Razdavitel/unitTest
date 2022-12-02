using Domain.Subscriptions;
using Domain.Payments;
using Domain.Exceptions;
using Domain.Users;
using Squads.Shared.Subscriptions;
using Squads.Shared.Users;
using Persistence.Data;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Services.Subscriptions;

public class SubscriptionService : ISubscriptionService
{

    private readonly SquadContext _ctx;
    private readonly DbSet<Subscription> _subscriptions;
    private readonly DbSet<Payment> _payments;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public SubscriptionService(SquadContext ctx, IMapper mapper, IUserService userService)
    {
        this._ctx = ctx;
        this._mapper = mapper;
        this._userService = userService;
        this._subscriptions = _ctx.Subscriptions;
        this._payments = _ctx.Payments;
    }

    public SubscriptionResponse.GetDetail Create(SubscriptionRequest.Create request)
    {
        SubscriptionResponse.GetDetail response = new();

        if(request.Subscription.customerId == null)
            throw new InvalidDataException("A subscription must be linked to a user");

        User customer = GetUserByIdOrError((int) request.Subscription.customerId);
        Subscription sub = customer.ExtendSubscription();
        Payment p = sub.CreatePayment();

        if(request.Subscription.ValidTill != null && request.Subscription.Price != null)
        {
            sub.ValidTill = (DateTime) request.Subscription.ValidTill;
            p.Price = (decimal) request.Subscription.Price;
        }

        _subscriptions.Add(sub);
        _payments.Add(p);
        _ctx.SaveChanges();
        response.Subscription = _mapper.Map<SubscriptionDto.Detail>(sub);

        return response;
    }

    public void Delete(SubscriptionRequest.Delete request)
    {
        Subscription subscription = GetByIdOrError(request.Id);
        _subscriptions.Remove(subscription);
        _ctx.SaveChanges();
    }

    public SubscriptionResponse.GetDetail Edit(SubscriptionRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public SubscriptionResponse.GetDetail Get(SubscriptionRequest.GetDetail request)
    {
        SubscriptionResponse.GetDetail response = new();
        Subscription subscription = GetByIdOrError(request.Id);
        response.Subscription = _mapper.Map<SubscriptionDto.Detail>(subscription);

        return response;
    }

    public SubscriptionResponse.GetDetail? GetLastSubscription(int userId)
    {
        SubscriptionResponse.GetDetail response = new();
        User user = GetUserByIdOrError(userId);
        Subscription last = user.GetLastSubscription();
        response.Subscription = _mapper.Map<SubscriptionDto.Detail>(last);
        return response;
    }

    public SubscriptionResponse.GetIndex GetList(SubscriptionRequest.GetIndex request)
    {
        SubscriptionResponse.GetIndex response = new();
        List<Subscription> subscriptions;
        if(request.UserId != null)
        {
            User user = GetUserByIdOrError((int) request.UserId);
            subscriptions = user.Subscriptions;
        }
        else
        {
            subscriptions = _subscriptions.ToList();
        }
        response.Subscriptions = subscriptions.Select(s => _mapper.Map<SubscriptionDto.Index>(s)).ToList();
        response.TotalAmount = subscriptions.Count();
        return response;
    }

    private IQueryable<Subscription> GetSubscripionById(int id) => _subscriptions
        .AsNoTracking()
        .Include(s => s.Customer)
        .Include(p => p.Payment)
        .Where(s => s.Id == id);

    private Subscription GetByIdOrError(int id)
    {
        Subscription? subscription = GetSubscripionById(id).SingleOrDefault();
        if(subscription == null)
            throw new EntityNotFoundException(nameof(Subscription), id);

        return subscription;
    }

    private User GetUserByIdOrError(int userId)
    {
        User? user =_ctx.Users
            .Include(u => u.ActivePricePlan)
            .Include(u => u.Subscriptions)
            .Where(u => u.Id == userId)
            .SingleOrDefault();

        if(user == null)    
            throw new EntityNotFoundException(nameof(User), userId);

        return user;
    }

}