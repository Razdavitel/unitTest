using Bogus;
using Domain.Users;

namespace Domain.Records;
public class RecordFaker : Faker<Record>
{
    public RecordFaker(User user)
    {
        CustomInstantiator(f => new Record(
                f.Random.Number(100, 200),
                user
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}
