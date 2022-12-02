using Squads.Shared.PricePlans;
using Domain.PricePlans;
using Domain.Exceptions;
using Persistence.Extensions;

using AutoMapper;
using Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Services.PricePlans;

public class PricePlanService : IPricePlanService
{
    private readonly SquadContext _dbctx;
    private readonly DbSet<PricePlan> _pricePlans;
    private readonly IMapper _mapper;

    public PricePlanService(SquadContext ctx, IMapper mapper)
    {
        _dbctx = ctx;
        _pricePlans = _dbctx.PricePlans;
        _mapper = mapper;
    }

    public async Task<PricePlanResponse.Create> CreateAsync(PricePlanRequest.Create request)
    {
        PricePlanResponse.Create reponse = new();
        PricePlan newPricePlan = _mapper.Map<PricePlan>(request.PricePlan);
        var pricePlan = _pricePlans.Add(newPricePlan);
        await _dbctx.SaveChangesAsync();
        reponse.PricePlan = _mapper.Map<PricePlanDto.Detail>(pricePlan.Entity);
        return reponse;
    }

    public async Task DeleteAsync(PricePlanRequest.Delete request)
    {
        _pricePlans.RemoveIf(p => p.Id == request.Id);
        await _dbctx.SaveChangesAsync();
    }

    public async Task<PricePlanResponse.Edit> EditAsync(PricePlanRequest.Edit request)
    {
        PricePlanResponse.Edit response = new();
        var pricePlan = await GetPricePlanById(request.Id).SingleOrDefaultAsync();

        if (pricePlan is null) return response;

        var model = request.ChangedPricePlan;
        pricePlan.endDate = model.endDate;
        pricePlan.SubscriptionPrice = model.SubscriptionPrice;
        pricePlan.Name = model.name;
        pricePlan.TurnPrice = model.TurnPrice;

        _dbctx.Entry(pricePlan).State = EntityState.Modified;
        await _dbctx.SaveChangesAsync();
        response.PricePlan = _mapper.Map<PricePlanDto.Detail>(pricePlan);
        return response;
    }

    public async Task<PricePlanResponse.GetDetail> GetDetailAsync(PricePlanRequest.GetDetail request)
    {
        PricePlanResponse.GetDetail response = new();
        var pricePlan = await GetPricePlanById(request.Id).SingleOrDefaultAsync();

        if (pricePlan is null)
            throw new EntityNotFoundException(nameof(PricePlan), request.Id);

        response.PricePlan = _mapper.Map<PricePlanDto.Detail>(pricePlan);
        return response;
    }

    public async Task<PricePlanResponse.GetIndex> GetIndexAsync(PricePlanRequest.GetIndex request)
    {
        PricePlanResponse.GetIndex response = new();
        var query = _pricePlans.AsQueryable().AsNoTracking();

        if(!string.IsNullOrWhiteSpace(request.SearchTerm))
            query.Where(p => p.Name.Contains(request.SearchTerm));

        response.TotalAmount = query.Count();
        if (request.Amount != null && request.Page != null)
        {
            query = query.Take(request.Amount ?? 0);
            query = query.Skip(request.Amount * request.Page ?? 0);
        }

        query.OrderBy(p => p.Name);
        response.PricePlans = await query.Select(p => _mapper.Map<PricePlanDto.Detail>(p)).ToListAsync();
        return response;
    }

    public async Task<PricePlanResponse.GetIndex> GetAll()
    {
        var plans = await _pricePlans.ToListAsync();
        PricePlanResponse.GetIndex response = new();
        response.TotalAmount = plans.Count;
        response.PricePlans = plans.Select(p => _mapper.Map<PricePlanDto.Detail>(p)).ToList();
        return response;
    }

    public PricePlan GetDefaultPricePlan()
    {
        var plan = _pricePlans.Where(p => p.Id == 1).First();
        return plan;
    }

    private IQueryable<PricePlan> GetPricePlanById(int id) => _pricePlans
        .AsNoTracking()
        .Where(u => u.Id == id);
} 