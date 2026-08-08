using Hotel.Domain.Folios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Folios;

public class FolioConfiguration : IEntityTypeConfiguration<Folio>
{
    public void Configure(EntityTypeBuilder<Folio> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.OriginatorId)
            .IsRequired();

        builder.Property(f => f.GuestId)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.HasMany(f => f.Items)
            .WithOne()
            .HasForeignKey(fi => fi.FolioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}