using Bogus;
using Domain.Exercises;

namespace Domain.Users;
public class ExerciseFaker : Faker<Exercise>
{
    public ExerciseFaker()
    {
        CustomInstantiator(f => new Exercise(
            0
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}