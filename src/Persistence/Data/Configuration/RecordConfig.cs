using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Records;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Bogus;

namespace Persistence.Data.Configuration;
public class RecordConfig : IEntityTypeConfiguration<Record>
{
    public void Configure(EntityTypeBuilder<Record> builder)
    {
        var splitStringConverter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
        builder.Property(r => r.Height).IsRequired();
        builder.Property(r => r.Medications).HasConversion(splitStringConverter);
        builder.Property(r => r.PhysicalIssues).HasConversion(splitStringConverter);
        builder.Property(r => r.UpdatedOn).ValueGeneratedOnAddOrUpdate();
        builder
            .Property(r => r.CreatedOn)
            .HasColumnType("timestamp without time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        builder.HasOne(r => r.Customer);
    }
}
