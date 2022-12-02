using Bogus;
using Domain.Users;

namespace Domain.Subscriptions;
public class SubscriptionFaker : Faker<Subscription>
{
    public SubscriptionFaker(User user)
    {
        CustomInstantiator(f => new Subscription(
                user,
                f.Date.Future()
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}
