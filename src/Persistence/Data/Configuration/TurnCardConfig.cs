using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.TurnCards;

namespace Persistence.Data.Configuration;
public class TurnCardConfig : IEntityTypeConfiguration<TurnCard>
{
    public void Configure(EntityTypeBuilder<TurnCard> builder)
    {
        builder.Property(r => r.NumberOfTurns).HasDefaultValue(10);
        builder.Property(r => r.UpdatedOn).ValueGeneratedOnAddOrUpdate();
        builder
            .Property(r => r.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        builder.HasOne(r => r.Customer).WithMany(u => u.TurnCards);
    }
}
