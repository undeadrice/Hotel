using Hotel.Domain.Folios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Folios;

public class FolioItemConfiguration : IEntityTypeConfiguration<FolioItem>
{
    public void Configure(EntityTypeBuilder<FolioItem> builder)
    {
        builder.HasKey(fi => fi.Id);

        builder.Property(fi => fi.FolioId)
            .IsRequired();

        builder.Property(fi => fi.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(fi => fi.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(fi => fi.CreatedAt)
            .IsRequired();
    }
}