using Domain.Sessions;
using Domain.Records;
using Domain.Users;
using Bogus;
using Domain.Reports;
using Domain.Workouts;
using Domain.PricePlans;
using Domain.Subscriptions;
using Domain.TurnCards;
using Domain.Payments;
using System.Collections.Generic;
using Domain.Common;

namespace Persistence.Data;
public class SquadDataInitializer
{
    private readonly SquadContext DbCtx;

    public SquadDataInitializer(SquadContext context)
    {
        DbCtx = context;
    }

    public void SeedData()
    {
        // TODO : fix failed migration updates because of this
       DbCtx.Database.EnsureDeleted();

        if (DbCtx.Database.EnsureCreated())
        {
            SeedPricePlan();
            var pricePlan = DbCtx.PricePlans.First();
            SeedUsers(pricePlan);
            var users = DbCtx.Users.ToList();
            SeedSubscriptions(users);
            SeedWorkouts();
            SeedSessions(users);
            SeedRecords(users);
            SeedReports(users);  
            var sessions = DbCtx.Sessions.ToList();
            SeedUsersInSessions(users, sessions);
            var workouts = DbCtx.Workouts.ToList();
            SeedWorkoutsInSessions(workouts, sessions);

        }
    }

    private void SeedPricePlan()
    {
        PricePlan pricePlan = new PricePlanFaker("default");
        DbCtx.PricePlans.Add(pricePlan);
        DbCtx.SaveChanges();
    }

    private void SeedUsers(PricePlan pricePlan)
    {
        var users = new UserFaker(pricePlan)
            .RuleFor(u => u.Id, () => 0) // Remove the id, database column is auto generated
            .Generate(10);
        DbCtx.Users.AddRange(users);
        DbCtx.Users.Add(new User("user", "user", "user@mail.com", DateTime.Now, users[0].PasswordHash, users[0].PasswordSalt, RoleType.Customer, pricePlan, UserStatus.Active));
        DbCtx.Users.Add(new User("admin", "admin", "admin@mail.com", DateTime.Now, users[0].PasswordHash, users[0].PasswordSalt, RoleType.Admin, pricePlan, UserStatus.Active));
        DbCtx.SaveChanges();
    }
    private void SeedSessions(List<User> users)
    {
        var random = new Random();
        //TODO: Should filter user list to coaches only.
        var sessions= new SessionFaker(users)
            .RuleFor(u => u.Id, () => 0) // Remove the id, database column is auto generated
            .Generate(20);
        DbCtx.Sessions.AddRange(sessions);
        DbCtx.SaveChanges();
    }
    private void SeedUsersInSessions(List<User> users, List<Session> ses)
    {
        ses.ForEach(session =>
        {
            Random r = new Random();
            int userCount = r.Next(0, 5);
            // inneficient but readable
            var paritcipants = users.OrderBy(x => r.Next()).Take(userCount).ToList();
            paritcipants.ForEach(participant =>
            {
                session.AddReservation(participant);
            });
        });

        var sessions = DbCtx.SaveChanges();
    }
    private void SeedRecords(List<User> users)
    {
        var records  = new List<Record>();
        var faker = new Faker();
        users.ForEach(user =>
        {
            var record = new RecordFaker(user)
                .RuleFor(u => u.Id, () => 0)
                .RuleFor(u => u.PhysicalIssues, () => {
                    var list = new List<string>();
                    list.Add(faker.Lorem.Word());
                    return list;
                }).RuleFor(u => u.Medications, () => {
                    var list = new List<string>();
                    list.Add(faker.Lorem.Word());
                    return list;
                });
            records.Add(record);
        });
        DbCtx.Records.AddRange(records);
        DbCtx.SaveChanges();
    }
    
    private void SeedReports(List<User> users)
    {
        var reports  = new List<Report>();
        users.ForEach(user =>
        {
            var report = new ReportFaker(user)
                .RuleFor(u => u.Id, () => 0);

            reports.Add(report);
        });


        DbCtx.Reports.AddRange(reports);
        DbCtx.SaveChanges();
    }
    private void SeedWorkouts()
    {
        var random = new Random();
        var workouts = new WorkoutFaker()
            .RuleFor(u => u.Id, () => 0) // Remove the id, database column is auto generated
            .Generate(20);

        DbCtx.Workouts.AddRange(workouts);
        DbCtx.SaveChanges();
    }
    private void SeedWorkoutsInSessions(List<Workout> wor, List<Session> ses)
    {
        ses.ForEach(session =>
        {
            Random r = new Random();
            int workoutCount = r.Next(0, 5);
            // inneficient but readable
            var workouts = wor.OrderBy(x => r.Next()).Take(workoutCount).ToList();
            workouts.ForEach(workout =>
            {
                session.Workout= workout;
            });
        });

        var sessions = DbCtx.SaveChanges();
    }

    private void SeedSubscriptions(List<User> users)
    {
        bool even = true;
        users.ForEach(u =>
        {
            if(even)
            {
                Subscription s = new SubscriptionFaker(u);
                DbCtx.Subscriptions.Add(s);
                Payment p = s.CreatePayment();
                DbCtx.Payments.Add(p);
            } 
            else 
            {
                TurnCard t = new TurnCardFaker(u);
                DbCtx.TurnCards.Add(t);
                Payment p = t.CreatePayment();
                DbCtx.Payments.Add(p);
            }
            DbCtx.SaveChanges();
            even = !even;
        });
    }
}
