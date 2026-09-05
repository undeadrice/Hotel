using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.FiscalAccounting;

public class FolioConfiguration : IEntityTypeConfiguration<Folio>
{
    public void Configure(EntityTypeBuilder<Folio> builder)
    {
        builder.ToTable("Folios");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedNever();

        builder.Property(f => f.FiscalAccountId)
            .IsRequired();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(FolioStatus.Open);

        builder.Property(f => f.IsMainFolio)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasMany(f => f.Items)
            .WithOne()
            .HasForeignKey(fi => fi.FolioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}