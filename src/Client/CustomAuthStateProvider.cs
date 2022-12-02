using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Squads.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = await _localStorage.GetItemAsStringAsync("token");

        var identity = new ClaimsIdentity(); // empty identity means not authorized

        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!String.IsNullOrEmpty(token))
        {
            var parsedToken  = ParseClaimsFromJwt(token);
            identity = new ClaimsIdentity(parsedToken, "jwt");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        }
        
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        // used by other components (login => logout btn) 
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
