using Ardalis.GuardClauses;
using Squads.Shared.Auth;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Squads.Client.Auth;

public class AuthService : IAuthService
{
    private readonly HttpClient client;
    private readonly ILocalStorageService localStorage;
    private readonly AuthenticationStateProvider authStateProvider;
    private const string endpoint = "api/auth";
    public AuthService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        this.client = client;
        this.localStorage = localStorage;
        this.authStateProvider = authStateProvider;
    }

    public async Task<AuthResponse.Login> LoginAsync(AuthRequest.Login request)
    {
        var res = await client.PostAsJsonAsync($"{endpoint}/login", request);
        var resContent = await res.Content.ReadFromJsonAsync<AuthResponse.Login>();
        Guard.Against.Null(resContent);
        await SaveTokenToLocalStorage(resContent.Token);
        return resContent;
    }

    public async Task<AuthResponse.Register> RegisterAsync(AuthRequest.Register request)
    {
        var res = await client.PostAsJsonAsync($"{endpoint}/register", request);
        var resContent = await res.Content.ReadFromJsonAsync<AuthResponse.Register>();
        Guard.Against.Null(resContent);
        await SaveTokenToLocalStorage(resContent.Token);
        return resContent;
    }

    public async Task<AuthResponse.Register> RegisterUserAsync(AuthRequest.Register request)
    {
        var res = await client.PostAsJsonAsync($"{endpoint}/register", request);
        var resContent = await res.Content.ReadFromJsonAsync<AuthResponse.Register>();
        Guard.Against.Null(resContent);
        return resContent;
    }


    private async Task SaveTokenToLocalStorage(string token)
    {
        Guard.Against.NullOrEmpty(token);
        await localStorage.SetItemAsync("token", token);
        await authStateProvider.GetAuthenticationStateAsync();
    }

    public async Task<AuthResponse.Current> CurrentAsync()
    {
        var res = await client.GetAsync($"{endpoint}/current");
        var resContent = await res.Content.ReadFromJsonAsync<AuthResponse.Current>();
        Guard.Against.Null(resContent);
        return resContent;
    }
}
