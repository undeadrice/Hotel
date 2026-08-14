using Hotel.Domain.FiscalAccounting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.FiscalAccounting;

public class FolioItemConfiguration : IEntityTypeConfiguration<FolioItem>
{
    public void Configure(EntityTypeBuilder<FolioItem> builder)
    {
        builder.ToTable("FolioItems");

        builder.HasKey(fi => fi.Id);

        builder.Property(fi => fi.Id)
            .ValueGeneratedNever();

        builder.Property(fi => fi.FolioId)
            .IsRequired();

        builder.Property(fi => fi.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(fi => fi.Quantity)
            .IsRequired();

        builder.Property(fi => fi.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(fi => fi.TransactionCodeId)
            .IsRequired();

        builder.Property(fi => fi.CreatedAt)
            .IsRequired();
    }
}