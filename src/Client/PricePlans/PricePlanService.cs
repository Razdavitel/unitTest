using Ardalis.GuardClauses;
using Squads.Shared.PricePlans;
using System.Net.Http.Json;

namespace Squads.Client.PricePlans;

public class PricePlanService : IPricePlanService
{
    private readonly HttpClient _client;
    private const string _ENDPOINT = "api/priceplan";

    public PricePlanService(HttpClient client)
    {
        _client = client;
    }

    public async Task<PricePlanResponse.Create> CreateAsync(PricePlanRequest.Create request)
    {
        var res = await _client.PostAsJsonAsync(_ENDPOINT, request);
        Guard.Against.Null(res);
        PricePlanResponse.Create? cont = await res.Content.ReadFromJsonAsync<PricePlanResponse.Create>();
        Guard.Against.Null(cont);
        return cont;
    }

    public async Task DeleteAsync(PricePlanRequest.Delete request)
    {
        await _client.DeleteAsync($"{_ENDPOINT}/{request.Id}");
    }

    public async Task<PricePlanResponse.Edit> EditAsync(PricePlanRequest.Edit request)
    {
        var res = await _client.PutAsJsonAsync(_ENDPOINT, request);
        Guard.Against.Null(res);
        PricePlanResponse.Edit? cont = await res.Content.ReadFromJsonAsync<PricePlanResponse.Edit>();
        Guard.Against.Null(cont);
        return cont;
    }

    public async Task<PricePlanResponse.GetDetail> GetDetailAsync(PricePlanRequest.GetDetail request)
    {
        PricePlanResponse.GetDetail? res = await _client.GetFromJsonAsync<PricePlanResponse.GetDetail>($"{_ENDPOINT}/{request.Id}");
        Guard.Against.Null(res);
        return res;
    }

    public async Task<PricePlanResponse.GetIndex> GetIndexAsync(PricePlanRequest.GetIndex request)
    {
       PricePlanResponse.GetIndex? res = await _client.GetFromJsonAsync<PricePlanResponse.GetIndex>($"{_ENDPOINT}");
       Guard.Against.Null(res);
       return res;
    }
}