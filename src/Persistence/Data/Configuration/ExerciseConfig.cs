using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Exercises;

namespace Persistence.Data.Configuration;
public class ExerciseConfig : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(w=>w.ExerciseType).IsRequired();
    }
}
