using Domain.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class SessionConfig : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(sc => sc.Title).IsRequired();
        builder.Property(sc => sc.EndsAt).IsRequired();
        builder.Property(sc => sc.StartsAt).IsRequired();
        builder.HasOne(sc => sc.Coach);
        builder.HasOne(sc => sc.Workout);
        builder.HasMany(sc => sc.Trainees);
        builder.Property(sc => sc.UpdatedOn).ValueGeneratedOnAddOrUpdate();
        builder
            .Property(sc => sc.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
    }
}
