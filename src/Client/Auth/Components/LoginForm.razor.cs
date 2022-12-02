#nullable disable
using Microsoft.AspNetCore.Components;
using Squads.Shared.Auth;

namespace Squads.Client.Auth.Components;

public partial class LoginForm
{
    private AuthDto.Login user = new AuthDto.Login();

    [Inject] public IAuthService AuthService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }


    async Task Submit()
    {
        var req = new AuthRequest.Login() { User = user };
        await AuthService.LoginAsync(req);
        NavigationManager.NavigateTo("/");
    }
}