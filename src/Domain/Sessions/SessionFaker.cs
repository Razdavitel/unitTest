using Bogus;
using Domain.Users;
using Domain.Workouts;

namespace Domain.Sessions;
public class SessionFaker : Faker<Session>
{
    public SessionFaker(List<User> users)
    {
        Random ran = new Random();
        var startDate = DateTime.Now.AddDays(ran.NextInt64(1,7));
        CustomInstantiator(f => new Session(
                f.Lorem.Sentence(), 
                f.Lorem.Paragraph(), 
                startDate,
                startDate.AddMinutes(100),
                users[ran.Next(users.Count)],
                new WorkoutFaker()
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int(0)));
    }
}
