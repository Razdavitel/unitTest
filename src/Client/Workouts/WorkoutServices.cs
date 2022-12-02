using Squads.Client.Extensions;
using Squads.Shared.Sessions;
using Squads.Shared.Workouts;
using System.Net.Http.Json;

namespace Squads.Client.Workouts;

public class WorkoutService:IWorkoutService
{
    private readonly HttpClient client;
    private const string endpoint = "api/workout";
    public WorkoutService(HttpClient client)
    {
        this.client = client;
    }
    public async Task<WorkoutResponse.Create> CreateAsync(WorkoutRequest.Create request)
    {
        var response = await client.PostAsJsonAsync(endpoint, request);
        return (await response.Content.ReadFromJsonAsync<WorkoutResponse.Create>())!;
    }

    public async Task DeleteAsync(WorkoutRequest.Delete request)
    {
        await client.DeleteAsync($"{endpoint}/{request.WorkoutId}");
    }

    public async Task<WorkoutResponse.GetDetail> GetDetailAsync(WorkoutRequest.GetDetail request)
    {
        var response = await client.GetFromJsonAsync<WorkoutResponse.GetDetail>($"{endpoint}/{request.WorkoutId}");
        return response!;
    }

    public async Task<WorkoutResponse.GetIndex> GetIndexAsync(WorkoutRequest.GetIndex request)
    {
        var queryParameters = request.GetQueryString();
        var response = await client.GetFromJsonAsync<WorkoutResponse.GetIndex>($"{endpoint}?{queryParameters}");
        return response!;
    }

    public async Task<WorkoutResponse.Edit> EditAsync(WorkoutRequest.Edit request)
    {
        var response = await client.PutAsJsonAsync(endpoint, request);
        return (await response.Content.ReadFromJsonAsync<WorkoutResponse.Edit>())!;
    }
}
