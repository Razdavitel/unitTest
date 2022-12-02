using Domain.TurnCards;
using Domain.Payments;
using Domain.Exceptions;
using Domain.Users;
using Squads.Shared.TurnCards;
using Persistence.Data;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Services.TurnCards;

public class TurnCardService : ITurnCardService
{

    private readonly SquadContext _ctx;
    private readonly DbSet<TurnCard> _turnCards;
    private readonly DbSet<Payment> _payments;
    private readonly IMapper _mapper;

    public TurnCardService(SquadContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
        _turnCards = _ctx.TurnCards;
        _payments = _ctx.Payments;
    }

    public TurnCardResponse.GetDetail Create(TurnCardRequest.Create request)
    {
        TurnCardResponse.GetDetail response = new();
        if(request.TurnCard.CustomerId == null)
            throw new EntityNotFoundException(nameof(User), request.TurnCard.CustomerId);
        User user = GetUserByIdOrError((int) request.TurnCard.CustomerId);
        
        TurnCard turncard = user.CreateTurnCard();
        Payment payment = turncard.CreatePayment();
        _ctx.SaveChanges();
        response.TurnCard = _mapper.Map<TurnCardDto.Detail>(turncard);
        return response;
    }

    public TurnCardResponse.GetDetail Edit(TurnCardRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public TurnCardResponse.GetDetail ConsumeTurn(int turnCardId)
    {
        TurnCardResponse.GetDetail response = new();
        TurnCard turnCard = GetByIdOrError(turnCardId);
        turnCard.ConsumeTurn();
        _ctx.Entry(turnCard).State = EntityState.Modified;
        _ctx.SaveChanges();
        response.TurnCard = _mapper.Map<TurnCardDto.Detail>(turnCard);
        return response;
    }

    public TurnCardResponse.GetDetail Get(TurnCardRequest.GetDetail request)
    {
        TurnCardResponse.GetDetail response = new();
        TurnCard turnCard = GetByIdOrError(request.Id);
        response.TurnCard = _mapper.Map<TurnCardDto.Detail>(turnCard);
        return response;
    }

    public TurnCardResponse.GetIndex GetList(TurnCardRequest.GetIndex request)
    {
        TurnCardResponse.GetIndex response = new();
        List<TurnCard> turnCards;
        if(request.UserId != null)
        {
            User user = GetUserByIdOrError((int) request.UserId);
            turnCards = user.TurnCards;
        } 
        else
        {
            turnCards = _turnCards.ToList();
        } 
        response.TurnCards = turnCards.Select(t => _mapper.Map<TurnCardDto.Index>(t)).ToList();
        response.TotalAmount = turnCards.Count();
        return response;
    }

    public void Delete(TurnCardRequest.Delete request)
    {
        TurnCard turnCard = GetByIdOrError(request.Id);
        _turnCards.Remove(turnCard);
        _ctx.SaveChanges();
    }

    private IQueryable<TurnCard> GetTurnCardById(int id) => _turnCards
        .AsNoTracking()
        .Include(t => t.Customer)
        .Include(t => t.Payment)
        .Where(u => u.Id == id);

    private TurnCard GetByIdOrError(int id)
    {
        TurnCard? turnCard = GetTurnCardById(id).SingleOrDefault();
        if(turnCard == null)
            throw new EntityNotFoundException(nameof(TurnCard), id);

        return turnCard;
    }

    private User GetUserByIdOrError(int userId)
    {
        User? user =_ctx.Users
            .Include(u => u.ActivePricePlan)
            .Include(u => u.TurnCards)
            .Where(u => u.Id == userId)
            .SingleOrDefault();

        if(user == null)    
            throw new EntityNotFoundException(nameof(User), userId);

        return user;
    }
}