using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Shared.Users;

namespace Squads.Client.Users.Components;

public partial class EditUserForm
{
    [Inject] ISnackbar? SnackBar {  get; set; }
    [Inject] IUserService UserService {  get; set; }
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public UserDto.Detail user { get; set; } = new UserDto.Detail();

    private void Cancel()
    {
        MudDialog!.Cancel();
    }

    private async Task EditUser()
    {
        var editedUser = new UserDto.Mutate()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber
        };
        var req = new UserRequest.Edit() { User = editedUser, UserId = user.Id};
        await UserService.EditAsync(req);
        SnackBar!.Add($"Klant: {user.FirstName} {user.LastName} is aangepast", Severity.Success);
        MudDialog!.Close(DialogResult.Ok(user.Id));
    }
}
