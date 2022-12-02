using Squads.Client.Extensions;
using Squads.Shared.Sessions;
using System.Net.Http.Json;

namespace Squads.Client.Sessions;

public class SessionService : ISessionService
{
    private readonly HttpClient client;
    private const string endpoint = "api/session";
    public SessionService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<SessionResponse.Create> CreateAsync(SessionRequest.Create request)
    {
        var response = await client.PostAsJsonAsync(endpoint, request);
        return (await response.Content.ReadFromJsonAsync<SessionResponse.Create>())!;
    }

    public async Task DeleteAsync(SessionRequest.Delete request)
    {
        await client.DeleteAsync($"{endpoint}/{request.SessionId}");
    }

    public async Task<SessionResponse.GetDetail> GetDetailAsync(SessionRequest.GetDetail request)
    {
        var response = await client.GetFromJsonAsync<SessionResponse.GetDetail>($"{endpoint}/{request.SessionId}");
        return response!;
    }

    public async Task<SessionResponse.GetIndex> GetIndexAsync(SessionRequest.GetIndex request)
    {
        var queryParameters = request.GetQueryString();
        var response = await client.GetFromJsonAsync<SessionResponse.GetIndex>($"{endpoint}?{queryParameters}");
        return response!;
    }

    public async Task<SessionResponse.Edit> EditAsync(SessionRequest.Edit request)
    {
        var response = await client.PutAsJsonAsync(endpoint, request);
        return (await response.Content.ReadFromJsonAsync<SessionResponse.Edit>())!;
    }

    public async Task<SessionResponse.MutateTrainee> AddTrainee(SessionRequest.MutateTrainee request)

    {
        var response = await client.PutAsJsonAsync($"{endpoint}/join", request);
        return (await response.Content.ReadFromJsonAsync<SessionResponse.MutateTrainee>())!;
    }

    public async Task<SessionResponse.MutateTrainee> RemoveTrainee(SessionRequest.MutateTrainee request)
    {
        var response = await client.PutAsJsonAsync($"{endpoint}/leave", request);
        return (await response.Content.ReadFromJsonAsync<SessionResponse.MutateTrainee>())!;
    }
}