using Hotel.Domain.Reservations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Reservations;

public class ReservationGuestConfiguration : IEntityTypeConfiguration<ReservationGuest>
{
    public void Configure(EntityTypeBuilder<ReservationGuest> builder)
    {
        builder.HasKey(rg => new { rg.ReservationId, rg.GuestId });

        builder.Property(rg => rg.ReservationId)
            .IsRequired();

        builder.Property(rg => rg.GuestId)
            .IsRequired();
    }
}