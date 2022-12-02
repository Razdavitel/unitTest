using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Shared.Users;

namespace Squads.Client.Users.Components;

public partial class DeleteForm
{
    [Inject] ISnackbar? SnackBar {  get; set; }
    [Inject] IUserService UserService {  get; set; }
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public UserDto.Index user { get; set; } = new UserDto.Index();

    private void Cancel()
    {
        MudDialog!.Cancel();
    }

    private async Task DeleteUser()
    {
        var req = new UserRequest.Delete();
        req.UserId = user.Id;
        await UserService.DeleteAsync(req);
        SnackBar!.Add($"Klant: {user.FirstName} {user.LastName} is verwijderd", Severity.Success);
        MudDialog!.Close(DialogResult.Ok(user.Id));
    }
}
