using Domain.Users;
using FluentValidation;
using Squads.Shared.Record;
using Squads.Shared.Sessions;
using Squads.Shared.PricePlans;

namespace Squads.Shared.Users;

public static class UserDto
{
    public class Index
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Role { get; set; }
        public PricePlanDto.Detail ActivePricePlan { get; set; }
    }

    public class Detail : Index
    {
        public DateTime DateOfBirth { get; set; }
        public String? Address { get; set; }
        public String? PhoneNumber { get; set; }
        public RecordDto.Index? Record { get; set;  }
        public List<SessionDto.Index>? Sessions { get; set; }
    }

    public class Create
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class Activate
    {
        public string Password { get; set; }
        public string? confirmPassword { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class Mutate
    {
        public string Email { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public String? Address { get; set; }
        public String? PhoneNumber { get; set; }
        public RoleType? Role { get; set; }

        public class Validator : AbstractValidator<Mutate>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty().Length(1, 250);
                RuleFor(x => x.LastName).NotEmpty().Length(1, 250);

            }
        }
    }
}
