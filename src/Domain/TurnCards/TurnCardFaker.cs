using Bogus;
using Domain.Users;

namespace Domain.TurnCards;
public class TurnCardFaker : Faker<TurnCard>
{
    public TurnCardFaker(User user)
    {
        CustomInstantiator(f => new TurnCard(
                user,
                f.Date.Future(),
                f.Random.Int(0, 10)
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}
