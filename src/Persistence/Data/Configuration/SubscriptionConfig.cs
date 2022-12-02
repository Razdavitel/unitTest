using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Subscriptions;

namespace Persistence.Data.Configuration;
public class SubscriptionConfig : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(r => r.UpdatedOn).ValueGeneratedOnAddOrUpdate();
        builder
            .Property(r => r.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        builder.HasOne(r => r.Customer).WithMany(u => u.Subscriptions);
    }
}
