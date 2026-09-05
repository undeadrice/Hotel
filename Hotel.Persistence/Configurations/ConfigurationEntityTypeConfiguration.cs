using Hotel.Domain.Configurations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Configurations;

public class ConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        builder.ToTable("Configuration");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.TimeZone)
            .HasConversion(
                tz => tz.Id,
                id => TimeZoneInfo.FindSystemTimeZoneById(id))
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.CurrentBusinessDate)
            .IsRequired();

        builder.Property(c => c.IsSeeded)
            .IsRequired()
            .HasDefaultValue(false);
    }
}