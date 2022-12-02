using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Shared.Auth;

namespace Squads.Client.Auth.Components;

partial class RegisterForm
{
    [Inject] public IAuthService AuthService { get; set; }
    [Inject] public ISnackbar Bar { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    private MudForm? form;
    private AuthDto.Register.Validator registerValidator = new AuthDto.Register.Validator();
    private AuthDto.Register? user = new AuthDto.Register();

    private async Task Submit()
    {
        await form.Validate();
        Console.WriteLine(user);
        if (form.IsValid)
        {
            var req = new AuthRequest.Register() { User = user };
            await AuthService.RegisterAsync(req);
            NavigationManager.NavigateTo("/");
        }
    }
}
