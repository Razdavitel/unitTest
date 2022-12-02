using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Workouts;

namespace Persistence.Data.Configuration;
public class WorkoutConfig : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.Property(w=>w.WorkoutType).IsRequired();
        builder.HasMany(w => w.Exercises).WithMany(e => e.Workouts);
    }
}
