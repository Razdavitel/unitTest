using Bogus;
namespace Domain.Workouts;
public class WorkoutFaker : Faker<Workout>
{
    public WorkoutFaker()
    {
        var random = new Random();  
        CustomInstantiator(f => new Workout(random.Next(0,2), DateTime.Now));
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}
