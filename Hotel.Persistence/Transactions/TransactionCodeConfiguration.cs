using Hotel.Domain.Transactions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hotel.Persistence.Transactions;

public class TransactionCodeConfiguration : IEntityTypeConfiguration<TransactionCode>
{
    public void Configure(EntityTypeBuilder<TransactionCode> builder)
    {
        builder.ToTable("TransactionCodes");

        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.TransactionGroupId)
            .IsRequired();

        builder.Property(tc => tc.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(tc => tc.Code)
            .IsUnique();

        builder.Property(tc => tc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(tc => tc.Description)
            .HasMaxLength(500);

        builder.Property(tc => tc.DefaultAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(tc => tc.IsActive)
            .IsRequired();
    }
}
