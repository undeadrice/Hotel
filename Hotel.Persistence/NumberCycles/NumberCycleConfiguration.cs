using Hotel.Domain.NumberCycles.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.NumberCycles;

public class NumberCycleConfiguration : IEntityTypeConfiguration<NumberCycle>
{
    public void Configure(EntityTypeBuilder<NumberCycle> builder)
    {
        builder.ToTable("NumberCycles");

        builder.HasKey(nc => nc.Id);

        builder.Property(nc => nc.Id)
            .ValueGeneratedNever();

        builder.Property(nc => nc.Topic)
            .IsRequired()
            .HasConversion<int>();

        builder.HasIndex(nc => nc.Topic)
            .IsUnique();

        builder.Property(nc => nc.Prefix)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(nc => nc.StartIndex)
            .IsRequired();

        builder.Property(nc => nc.CurrentIndex)
            .IsRequired();

        builder.Property(nc => nc.CreatedAt)
            .IsRequired();
    }
}