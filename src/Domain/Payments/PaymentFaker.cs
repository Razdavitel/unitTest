using Bogus;
using Domain.Subscriptions;
using Domain.TurnCards;

namespace Domain.Payments;

public class PaymentFaker : Faker<Payment>
{
    public PaymentFaker(Subscription subscription)
    {
        CustomInstantiator(f => new Payment(
            f.Finance.Amount() % 100,
            subscription
        ));
        RuleFor(x => x.Id, f => f.Random.Int());
    }

    public PaymentFaker(TurnCard turnCard)
    {
        CustomInstantiator(f => new Payment(
            f.Finance.Amount() % 100,
            turnCard
        ));
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}