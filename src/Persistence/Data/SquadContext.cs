using Domain.Exercises;
using Domain.Records;
using Domain.Reports;
using Domain.Sessions;
using Domain.Users;
using Domain.Workouts;
using Domain.PricePlans;
using Domain.Subscriptions;
using Domain.TurnCards;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Configuration;

namespace Persistence.Data;
public class SquadContext : DbContext
{
    public DbSet<Session> Sessions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Record> Records { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<PricePlan> PricePlans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<TurnCard> TurnCards { get; set; }
    public DbSet<Payment> Payments { get; set; }

    public SquadContext(DbContextOptions<SquadContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new SessionConfig());
        modelBuilder.ApplyConfiguration(new RecordConfig());
        modelBuilder.ApplyConfiguration(new ReportConfig());
        modelBuilder.ApplyConfiguration(new TurnCardConfig());
        modelBuilder.ApplyConfiguration(new SubscriptionConfig());
        modelBuilder.ApplyConfiguration(new ExerciseConfig());
        modelBuilder.ApplyConfiguration(new WorkoutConfig());
        modelBuilder.ApplyConfiguration(new PricePlanConfig());
        modelBuilder.ApplyConfiguration(new PaymentConfig());
        //...Additional type configurations
    }
}
