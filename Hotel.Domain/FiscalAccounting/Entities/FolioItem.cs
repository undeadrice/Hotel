using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Entities;

public class FolioItem
{
    public Guid Id { get; private set; }
    public Guid FolioId { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal Amount { get; private set; }
    public Guid TransactionCodeId { get; private set; }
    public FolioItemType TransactionType { get; private set; }
    public DateOnly BusinessDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

#pragma warning disable CS8618
    public FolioItem() { }
#pragma warning restore CS8618

    private FolioItem(
        Guid id,
        Guid folioId,
        string description,
        int quantity,
        decimal amount,
        Guid transactionCodeId,
        FolioItemType transactionType,
        DateOnly businessDate,
        DateTime createdAt)
    {
        Id = id;
        FolioId = folioId;
        Description = description;
        Quantity = quantity;
        Amount = amount;
        TransactionCodeId = transactionCodeId;
        TransactionType = transactionType;
        BusinessDate = businessDate;
        CreatedAt = createdAt;
    }

    public static FolioItem Create(
        Guid folioId,
        string description,
        int quantity,
        decimal amount,
        Guid transactionCodeId,
        FolioItemType transactionType,
        DateOnly businessDate)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidFolioItemDescriptionException();
        }

        if (quantity <= 0)
        {
            throw new InvalidFolioItemQuantityException();
        }

        if (amount < 0)
        {
            throw new InvalidFolioItemAmountException();
        }

        return new FolioItem(
            Guid.NewGuid(),
            folioId,
            description,
            quantity,
            amount,
            transactionCodeId,
            transactionType,
            businessDate,
            DateTime.UtcNow);
    }
}