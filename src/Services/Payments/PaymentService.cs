using Squads.Shared.Payments;
using Squads.Shared.Users;
using Persistence.Data;
using Persistence.Extensions;
using Domain.Payments;
using Domain.Exceptions;
using Domain.Subscriptions;
using Domain.TurnCards;
using Domain.Users;

using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Services.Payments;

public class PaymentService : IPaymentService
{
    private readonly SquadContext _ctx;
    private readonly DbSet<Payment> _payments;
    private readonly IMapper _mapper;

    public PaymentService(SquadContext ctx, IMapper mapper)
    {
        this._ctx = ctx;
        this._mapper = mapper;
        _payments = ctx.Payments;
    }
    public PaymentResponse.GetDetail Create(PaymentRequest.Create request)
    {
        PaymentResponse.GetDetail response = new();
        Payment newPayment = _mapper.Map<Payment>(request.Payment);
        var payment = _payments.Add(newPayment);
        _ctx.SaveChanges();
        response.Payment = _mapper.Map<PaymentDto.Detail>(payment.Entity);
        return response;
    }

    public void Delete(PaymentRequest.Delete request)
    {
        Payment payment = GetByIdOrError(request.Id);
        _payments.Remove(payment);
        _ctx.SaveChanges();
    }

    public PaymentResponse.GetDetail Edit(PaymentRequest.Edit request)
    {
        Payment payment = GetByIdOrError(request.Id);
        PaymentResponse.GetDetail response = new();

        if(request.Payment.SubscriptionId != null && request.Payment.TurnCardId != null)
            throw new InvalidDataException("A payment can only be linked to either a subscription or a turncard");

        if(request.Payment.Price != null)
            payment.Price = (decimal) request.Payment.Price;
        
        if(request.Payment.SubscriptionId != null)
            payment.SubscriptionId = request.Payment.SubscriptionId;

        if(request.Payment.TurnCardId != null)
            payment.TurnCardId = request.Payment.TurnCardId;


        _ctx.Entry(payment).State = EntityState.Modified;
        _ctx.SaveChanges();
        return response;
    }

    public PaymentResponse.GetDetail Get(PaymentRequest.GetDetail request)
    {
        PaymentResponse.GetDetail response = new();
        Payment payment = GetByIdOrError(request.Id);

        response.Payment = _mapper.Map<PaymentDto.Detail>(payment);
        return response;
    }

    public PaymentResponse.GetIndex GetList(PaymentRequest.GetIndex request)
    {
        PaymentResponse.GetIndex response = new();
        List<Payment> payments;
        if(request.userId != null)
        {
            User user = GetUserByIdOrError((int) request.userId);
            payments = user.GetPayments();
        }
        else
        {
            payments =  _payments.ToList();
        }

        response.Payments = payments.Select(p => _mapper.Map<PaymentDto.Index>(p)).ToList();
        response.TotalAmount = payments.Count();
        return response;
    }

    public async Task<PaymentResponse.GetDetail> MarkAsPaid(int paymentId)
    {
        PaymentResponse.GetDetail response = new();
        Payment payment = GetByIdOrError(paymentId);
        payment.PayedAt = DateTime.Now;
        _ctx.Entry(payment).State = EntityState.Modified;
        _ctx.SaveChanges();
        response.Payment = _mapper.Map<PaymentDto.Detail>(payment);
        return response;
    }

    public async Task<PaymentResponse.GetWithUser> GetPayments()
    {
        List<PaymentDto.WithUser> list = _ctx.Subscriptions
            .Include(s => s.Customer)
            .Include(s => s.Payment)
            .Select(s => new PaymentDto.WithUser() {
                Id = s.Payment.Id,
                Price = s.Payment.Price, 
                CreatedOn = s.Payment.CreatedOn,
                PayedAt = s.Payment.PayedAt,
                User = _mapper.Map<UserDto.Index>(s.Customer)
            }).ToList();

        list.AddRange(
            _ctx.TurnCards
                .Include(t => t.Customer)
                .Include(t => t.Payment)
                .Select(t => new PaymentDto.WithUser(){
                    Id = t.Payment.Id,
                    Price = t.Payment.Price,
                    CreatedOn = t.Payment.CreatedOn,
                    PayedAt = t.Payment.PayedAt,
                    User = _mapper.Map<UserDto.Index>(t.Customer)
                })
        );

        PaymentResponse.GetWithUser res = new() {Payments = list, TotalAmount = list.Count};
        return res;
    }

    private IQueryable<Payment> GetPaymentById(int id) => _payments
        .AsNoTracking()
        .Where(u => u.Id == id);

    private Payment GetByIdOrError(int id)
    {
        Payment? payment = GetPaymentById(id).SingleOrDefault();
        if(payment == null)
            throw new EntityNotFoundException(nameof(Payment), id);
        return payment;
    }

    private User GetUserByIdOrError(int userId)
    {
        User? user =_ctx.Users.AsNoTracking()
                .Include(u => u.ActivePricePlan)
                .Include(u => u.Subscriptions).ThenInclude(s => s.Payment)
                .Include(u => u.TurnCards).ThenInclude(t => t.Payment)
                .Where(u => u.Id == userId)
                .SingleOrDefault();

        if(user == null)    
            throw new EntityNotFoundException(nameof(User), userId);

        return user;
    }
}