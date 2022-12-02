using Domain.Records;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;
public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).IsRequired();
        builder.Property(u => u.LastName).IsRequired();
        builder.HasMany(u => u.Sessions).WithMany(s => s.Trainees);
        builder.HasOne(u => u.ActivePricePlan).WithMany().IsRequired();
        builder.Property(u => u.UpdatedOn).ValueGeneratedOnAddOrUpdate();
        builder
            .Property(u => u.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
    }
}
