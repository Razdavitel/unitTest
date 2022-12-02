using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Reports;

namespace Persistence.Data.Configuration;
public class ReportConfig : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(r => r.Weight).IsRequired();
        builder.Property(r => r.FatPercentage).IsRequired();
        builder.Property(r => r.MusclePercentage).IsRequired();
        builder.Property(r => r.WaistSize).IsRequired();
        builder.HasOne(r => r.Customer);
        builder
            .Property(r => r.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
    }
}