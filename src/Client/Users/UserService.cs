using Ardalis.GuardClauses;
using Squads.Client.Extensions;
using Squads.Shared.Users;
using System.Net.Http.Json;

namespace Squads.Client.Users;

public class UserService : IUserService
{
    private readonly HttpClient client;
    private const string endpoint = "api/user";

    public UserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserResponse.Create> CreateAsync(UserRequest.Create req)
    {
        var res = await client.PostAsJsonAsync("api/auth/register", req);
        var resContent = await res.Content.ReadFromJsonAsync<UserResponse.Create>();
        Guard.Against.Null(resContent);
        return resContent;
    }

    public async Task DeleteAsync(UserRequest.Delete req)
    {
        await client.DeleteAsync($"{endpoint}/{req.UserId}");
    }

    public async Task<UserResponse.Edit> EditAsync(UserRequest.Edit req)
    {
        var res = await client.PutAsJsonAsync(endpoint, req);
        Guard.Against.Null(res);
        return await res.Content.ReadFromJsonAsync<UserResponse.Edit>();
    }

    public async Task<UserResponse.GetDetail> GetDetailAsync(UserRequest.GetDetail req)
    {
        var res = await client.GetFromJsonAsync<UserResponse.GetDetail>($"{endpoint}/{req.UserId}");
        Guard.Against.Null(res);
        return res;
    }
    public async Task<UserResponse.GetDetail> GetDetailByEmailAsync(UserRequest.GetDetail request)
    {
        var queryParameters = request.GetQueryString();
        var response = await client.GetFromJsonAsync<UserResponse.GetDetail>($"{endpoint}/mail/{request.Email}");
        return response!;
    }


    public async Task<UserResponse.GetIndex> GetIndexAsync(UserRequest.GetIndex req)
    {
        var queryParameters = req.GetQueryString();
        var res = await client.GetFromJsonAsync<UserResponse.GetIndex>($"{endpoint}?{queryParameters}");
        Guard.Against.Null(res);
        return res;
    }

    public async Task<UserResponse.GetDetail> UpdatePricePlan(int userId, int pricePlanId)
    {
        UserRequest.Edit req = new();
        req.UserId = userId;
        var res = await client.PutAsJsonAsync($"{endpoint}/{userId}/updatePricePlan/{pricePlanId}", req);
        Guard.Against.Null(res);
        var cont = await res.Content.ReadFromJsonAsync<UserResponse.GetDetail>();
        Guard.Against.Null(cont);
        return cont;
    }

    public async Task<bool> InviteUser(UserRequest.InviteUser request)
    {
        var res = await client.PostAsJsonAsync($"{endpoint}/InviteUser", request);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> ActivateUser(string token, UserRequest.ActivateUser request)
    {
        var res = await client.PutAsJsonAsync($"{endpoint}/activate/{token}", request);
        return res.IsSuccessStatusCode;
    }
}
