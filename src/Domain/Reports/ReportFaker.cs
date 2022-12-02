using Bogus;
using Domain.Users;

namespace Domain.Reports;
public class ReportFaker : Faker<Report>
{
    public ReportFaker(User user)
    {
        CustomInstantiator(f => new Report(
                user,
                f.Random.Double(60, 100),
                f.Random.Double(6.0, 25.0),
                f.Random.Double(6.0, 25.0),
                f.Random.Double(30, 50)    
            )
        );
        RuleFor(x => x.Id, f => Math.Abs(f.Random.Int()));
    }
}
