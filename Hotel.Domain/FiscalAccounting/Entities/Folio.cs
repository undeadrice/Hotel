namespace Hotel.Domain.FiscalAccounting.Entities;

public class Folio
{
    public Guid Id { get; private set; }
    public Guid FiscalAccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<FolioItem> _items = new();
    public IReadOnlyCollection<FolioItem> Items => _items.AsReadOnly();

#pragma warning disable CS8618
    public Folio() { }
#pragma warning restore CS8618

    private Folio(
        Guid id,
        Guid fiscalAccountId,
        DateTime createdAt)
    {
        Id = id;
        FiscalAccountId = fiscalAccountId;
        CreatedAt = createdAt;
    }

    internal static Folio Create(Guid fiscalAccountId)
    {
        return new Folio(
            Guid.NewGuid(),
            fiscalAccountId,
            DateTime.UtcNow);
    }

    public FolioItem AddItem(
        string description,
        int quantity,
        decimal amount,
        Guid transactionCodeId)
    {
        var item = FolioItem.Create(Id, description, quantity, amount, transactionCodeId);
        _items.Add(item);
        return item;
    }
}
