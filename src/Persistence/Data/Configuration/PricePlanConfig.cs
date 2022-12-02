using Domain.PricePlans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configuration;

public class PricePlanConfig : IEntityTypeConfiguration<PricePlan>
{
    public void Configure(EntityTypeBuilder<PricePlan> builder)
    {
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.SubscriptionPrice).IsRequired();
        builder.Property(p => p.TurnPrice).IsRequired();
    }
}