using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Reservations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.CreatorId)
            .IsRequired();

        builder.Property(r => r.RoomId)
            .IsRequired();

        builder.Property(r => r.RatePlanId)
            .IsRequired();

        builder.Property(r => r.CycleIdentifier)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.StartDate)
            .IsRequired();

        builder.Property(r => r.EndDate)
            .IsRequired();

        builder.Property(r => r.ArrivalTime)
            .IsRequired(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(ReservationStatus.Reserved);

        builder.HasMany(r => r.Guests)
            .WithOne()
            .HasForeignKey(g => g.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}