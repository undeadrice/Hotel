using Hotel.Domain.RatePlans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.RatePlans;

public class RatePlanConfiguration : IEntityTypeConfiguration<RatePlan>
{
    public void Configure(EntityTypeBuilder<RatePlan> builder)
    {
        builder.ToTable("RatePlans");

        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(rp => rp.TransactionCodeId)
            .IsRequired();

        builder.Property(rp => rp.StartDate)
            .IsRequired();

        builder.Property(rp => rp.EndDate)
            .IsRequired();

        builder.Property(rp => rp.IsActive)
            .IsRequired();

        builder.HasMany(rp => rp.Rooms)
            .WithOne()
            .HasForeignKey(rpr => rpr.RatePlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}