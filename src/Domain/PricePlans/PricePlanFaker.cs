using Bogus;

namespace Domain.PricePlans;

public class PricePlanFaker : Faker<PricePlan>
{
    public PricePlanFaker(string name)
    {
        CustomInstantiator(f => new PricePlan(
            name,
            f.Finance.Amount() % 100,
            f.Finance.Amount() % 10
        ));
        RuleFor(x => x.Id, f => 1);
    }
}