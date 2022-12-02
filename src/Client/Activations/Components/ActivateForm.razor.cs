using Squads.Shared.Users;
using MudBlazor;
using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace Squads.Client.Activations.Components;

public partial class ActivateForm
{
    private UserDto.Activate _activateUser = new();
    private MudForm _form { get; set; }
    private bool _isValid { get; set; }
    private string[] _errors { get; set; }
    private ActivateUserValidator _validator = new();

    [Inject] IUserService _userService { get; set; }

    [Parameter] 
    public string? Token { get; set; }
    [Inject] ISnackbar SnackBar { get; set; }
    [Inject] NavigationManager NavManager { get; set; }

    bool _loading = false;

    public async Task Activate()
    {
        await _form.Validate();

        if(_form.IsValid)
        {
            _loading = true;
            UserRequest.ActivateUser request = new();
            request.User = _activateUser;
            
            bool success = await _userService.ActivateUser(Token, request);
            if(success)
            {
                SnackBar.Add("Succesvol geactiveerd.", MudBlazor.Severity.Success);
                NavManager.NavigateTo("/");
            }
            else
            {
                SnackBar.Add("Een probleem deed zich voor bij het activeren.", MudBlazor.Severity.Error);
                _loading = false;
            }
        }
    }
    
    public class ActivateUserValidator : AbstractValidator<UserDto.Activate>
    {
        public ActivateUserValidator()
        {
            RuleFor(u => u.Address)
                .NotEmpty()
                .Length(1,250);

            RuleFor(u => u.Password)
                .NotEmpty();
            
            RuleFor(u => u.confirmPassword)
                .Must((u, confirm) => u.Password == confirm)
                .WithMessage("Het bevestigde wachtwoord komt niet overeen met oorspronkelijke.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {   
            var result = await ValidateAsync(ValidationContext<UserDto.Activate>.CreateWithOptions((UserDto.Activate)model, x => x.IncludeProperties(propertyName)));

            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        }; 
    }
}