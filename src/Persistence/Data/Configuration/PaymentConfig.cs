using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Payments;

namespace Persistence.Data.Configuration;
public class PaymentConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.UpdatedOn).ValueGeneratedOnAddOrUpdate();
        builder
            .Property(p => p.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        
        builder.HasOne(p => p.Subscription)
            .WithOne(s => s.Payment)
            .HasForeignKey<Payment>(p => p.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.TurnCard)
            .WithOne(t => t.Payment)
            .HasForeignKey<Payment>(p => p.TurnCardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
