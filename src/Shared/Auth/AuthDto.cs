using FluentValidation;
using Squads.Shared.Users;

namespace Squads.Shared.Auth;
public static class AuthDto
{
    public class Login
    {

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }

    public class Register : UserDto.Mutate
    {
        public new DateTime? DateOfBirth { get; set; }
        public new string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;

        public new class Validator : AbstractValidator<Register>
        {
                public Validator()
                {
                    RuleFor(x => x.Email)
                        .NotEmpty()
                        .EmailAddress()
                        .MustAsync(async (value, cancellationToken) => await IsUniqueAsync(value));
                    RuleFor(x => x.Password).NotEmpty();
                    RuleFor(x => new { x.Password, x.PasswordConfirmation }).Must(x => x.Password.Equals(x.PasswordConfirmation));
                    RuleFor(x => x.FirstName).NotEmpty().Length(1, 250);
                    RuleFor(x => x.LastName).NotEmpty().Length(1, 250);
                }

                private async Task<bool> IsUniqueAsync(string email)
                {
                    // Simulates a long running http call
                    await Task.Delay(2000);
                    return email.ToLower() != "test@test.com";
                }

                public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
                {
                    var result = await ValidateAsync(ValidationContext<Register>.CreateWithOptions((Register)model, x => x.IncludeProperties(propertyName)));
                    if (result.IsValid)
                        return Array.Empty<string>();
                    return result.Errors.Select(e => e.ErrorMessage);
                };
        }
    }
}
