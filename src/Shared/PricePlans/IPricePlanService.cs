namespace Squads.Shared.PricePlans;

public interface IPricePlanService
{
    Task<PricePlanResponse.GetIndex> GetIndexAsync(PricePlanRequest.GetIndex request);
    Task<PricePlanResponse.GetDetail> GetDetailAsync(PricePlanRequest.GetDetail request);
    Task DeleteAsync(PricePlanRequest.Delete request);
    Task<PricePlanResponse.Create> CreateAsync(PricePlanRequest.Create request);
    Task<PricePlanResponse.Edit> EditAsync(PricePlanRequest.Edit request);
}
