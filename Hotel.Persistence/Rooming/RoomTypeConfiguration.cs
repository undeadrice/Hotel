using Hotel.Domain.Rooming.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Rooming;

public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("RoomTypes");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rt => rt.Description)
            .HasMaxLength(500);

        builder.Property(rt => rt.IsActive)
            .IsRequired();
    }
}