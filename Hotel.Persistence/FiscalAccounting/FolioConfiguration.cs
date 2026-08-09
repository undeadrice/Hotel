using Hotel.Domain.FiscalAccounting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.FiscalAccounting;

public class FolioConfiguration : IEntityTypeConfiguration<Folio>
{
    public void Configure(EntityTypeBuilder<Folio> builder)
    {
        builder.ToTable("Folios");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FiscalAccountId)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.HasMany(f => f.Items)
            .WithOne()
            .HasForeignKey(fi => fi.FolioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}