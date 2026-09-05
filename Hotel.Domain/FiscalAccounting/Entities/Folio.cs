using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Entities;

public class Folio
{
    public Guid Id { get; private set; }
    public Guid FiscalAccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public FolioStatus Status { get; private set; }
    public bool IsMainFolio { get; private set; }

    private readonly List<FolioItem> _items = new();
    public IReadOnlyCollection<FolioItem> Items => _items.AsReadOnly();

#pragma warning disable CS8618
    public Folio() { }
#pragma warning restore CS8618

    private Folio(
        Guid id,
        Guid fiscalAccountId,
        DateTime createdAt,
        bool isMainFolio)
    {
        Id = id;
        FiscalAccountId = fiscalAccountId;
        CreatedAt = createdAt;
        Status = FolioStatus.Open;
        IsMainFolio = isMainFolio;
    }

    internal static Folio Create(Guid fiscalAccountId, DateTime createdAt, bool isMainFolio)
    {
        return new Folio(
            Guid.NewGuid(),
            fiscalAccountId,
            createdAt,
            isMainFolio);
    }

    public FolioItem AddItem(
        string description,
        int quantity,
        decimal amount,
        Guid transactionCodeId,
        FolioItemType transactionType,
        DateOnly businessDate,
        DateTime createdAt)
    {
        var item = FolioItem.Create(
            Id,
            description,
            quantity,
            amount,
            transactionCodeId,
            transactionType,
            businessDate,
            createdAt);

        _items.Add(item);
        return item;
    }

    public void Settle()
    {
        if (Status == FolioStatus.Settled)
        {
            throw new FolioAlreadySettledException();
        }

        var total = 0m;

        foreach (var item in _items)
        {
            total += item.TransactionType == FolioItemType.Charge
                ? item.Amount * item.Quantity
                : -item.Amount * item.Quantity;
        }

        if (total != 0m)
        {
            throw new FolioNotBalancedException();
        }

        Status = FolioStatus.Settled;
    }
}