using Hotel.Domain.Transactions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Transactions;

public class TransactionGroupConfiguration : IEntityTypeConfiguration<TransactionGroup>
{
    public void Configure(EntityTypeBuilder<TransactionGroup> builder)
    {
        builder.ToTable("TransactionGroups");

        builder.HasKey(tg => tg.Id);

        builder.Property(tg => tg.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(tg => tg.Code)
            .IsUnique();

        builder.Property(tg => tg.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tg => tg.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(tg => tg.IsActive)
            .IsRequired();

        builder.HasMany(tg => tg.TransactionCodes)
            .WithOne()
            .HasForeignKey(tc => tc.TransactionGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata
            .FindNavigation(nameof(TransactionGroup.TransactionCodes))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
