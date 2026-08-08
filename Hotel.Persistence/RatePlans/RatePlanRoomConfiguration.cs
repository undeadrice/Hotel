using Hotel.Domain.RatePlans.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.RatePlans;

public class RatePlanRoomConfiguration : IEntityTypeConfiguration<RatePlanRoom>
{
    public void Configure(EntityTypeBuilder<RatePlanRoom> builder)
    {
        builder.ToTable("RatePlanRooms");

        builder.HasKey(rpr => new { rpr.RatePlanId, rpr.RoomTypeId });

        builder.Property(rpr => rpr.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}