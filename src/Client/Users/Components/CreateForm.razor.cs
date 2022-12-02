using Microsoft.AspNetCore.Components;
using MudBlazor;
using Squads.Shared.Auth;
using Squads.Shared.Users;

using FluentValidation;

namespace Squads.Client.Users.Components;

public partial class CreateForm
{
    [Inject] ISnackbar? SnackBar { get; set; }
    [Inject] IUserService UserService {  get; set; }
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    bool Loading = false;

    MudForm Form;
    bool Success;
    string[] Errors;
    UserDto.Create Create = new();
    UserCreateValidator Validator = new();

    public AuthDto.Register user { get; set; } = new AuthDto.Register();

    private void Cancel()
    {
        MudDialog!.Cancel();
    }

    private async Task CreateUser()
    {
        Loading = true;
        await Form.Validate();

        if(Form.IsValid)
        {
            UserRequest.InviteUser req = new();
            req.User = Create;
            bool success = await UserService.InviteUser(req);
            if(success)
            {
                SnackBar!.Add($"Klant: {Create.FirstName} {Create.LastName} is aangemaakt", MudBlazor.Severity.Success);
                MudDialog!.Close(DialogResult.Ok(Create));
            }
            else
            {
                SnackBar!.Add($"Probleem bij aanmaken klant", MudBlazor.Severity.Error);
            }
        }
        Loading = false;
    }

    class UserCreateValidator : AbstractValidator<UserDto.Create>
    {
        public UserCreateValidator()
        {
            RuleFor(u => u.Email) 
                .NotEmpty()
                .Length(1,100);

            RuleFor(u => u.FirstName) 
                .NotEmpty()
                .Length(1,100);

            RuleFor(u => u.LastName) 
                .NotEmpty()
                .Length(1,100);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {   
            var result = await ValidateAsync(ValidationContext<UserDto.Create>.CreateWithOptions((UserDto.Create)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
