using Squads.Shared.PricePlans;
using Ardalis.GuardClauses;

namespace Squads.Client.PricePlans;

public class PricePlanState 
{
    private IPricePlanService _pricePlanService { get; set; }
    public List<PricePlanDto.Detail> PricePlans { get; set; }
    public event Action OnChange;

    public PricePlanState(IPricePlanService service)
    {
        _pricePlanService = service;
    }

    public async Task GetPricePlans()
    {
        PricePlanRequest.GetIndex req = new();
        var res = await _pricePlanService.GetIndexAsync(req);
        PricePlans = res.PricePlans;
        NotifyStateChanged();
    }

    public async Task AddPricePlan(PricePlanDto.Mutate pricePlan)
    {
        PricePlanRequest.Create req = new();
        req.PricePlan = pricePlan;
        PricePlanResponse.Create res = await _pricePlanService.CreateAsync(req);
        PricePlans.Add(res.PricePlan);
        NotifyStateChanged();
    }

    public void DeletePricePlan(PricePlanDto.Detail pricePlan)
    {
        PricePlanRequest.Delete req = new() {Id = pricePlan.Id};
        _pricePlanService.DeleteAsync(req);
        PricePlans.Remove(pricePlan);
        NotifyStateChanged();
    }

    public void UpdatePricePlan(int id, PricePlanDto.Mutate pricePlan)
    {
        PricePlanRequest.Edit req = new() {Id=id, ChangedPricePlan = pricePlan};
        _pricePlanService.EditAsync(req);
        PricePlanDto.Detail? p = PricePlans.Find(p => p.Id == id);
        Guard.Against.Null(p);
        p.name = pricePlan.name;
        p.SubscriptionPrice = pricePlan.SubscriptionPrice;
        p.TurnPrice = pricePlan.TurnPrice;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnChange.Invoke();
    }
}