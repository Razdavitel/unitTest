using Bogus;
using Domain.Common;
using Domain.PricePlans;

namespace Domain.Users;
public class UserFaker : Faker<User> 
{
    public UserFaker(PricePlan pricePlan)
    {
        CustomInstantiator(f => new User(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Person.Email, 
                f.Person.DateOfBirth,
                PasswordFaker.GetFakeHash(),
                PasswordFaker.GetFakeSalt(),
                RoleType.Customer,
                pricePlan,
                UserStatus.Active
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
        RuleFor(x => x.PhoneNumber, f => "0470/123456");
        RuleFor(x => x.Address, f => "4455 Stad Straat");
    }
}
