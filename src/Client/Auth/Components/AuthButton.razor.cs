using Microsoft.AspNetCore.Components;

namespace Squads.Client.Auth.Components;

public partial class AuthButton
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; }

    private void Login() {
        NavigationManager.NavigateTo("login");
    }

    private async Task Logout() {
        await LocalStorage.RemoveItemAsync("token");
        await AuthStateProvider.GetAuthenticationStateAsync();
    } 

}
