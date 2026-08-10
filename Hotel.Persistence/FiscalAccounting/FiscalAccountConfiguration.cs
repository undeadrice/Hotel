using Hotel.Domain.FiscalAccounting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.FiscalAccounting;

public class FiscalAccountConfiguration : IEntityTypeConfiguration<FiscalAccount>
{
    public void Configure(EntityTypeBuilder<FiscalAccount> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.OriginatorId)
            .IsRequired();

        builder.Property(a => a.OwnerId)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.HasMany(a => a.Folios)
            .WithOne()
            .HasForeignKey(f => f.FiscalAccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}