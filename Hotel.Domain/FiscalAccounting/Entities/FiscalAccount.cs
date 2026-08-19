using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.Domain.FiscalAccounting.Exceptions;

namespace Hotel.Domain.FiscalAccounting.Entities;

public class FiscalAccount
{
    public Guid Id { get; private set; }
    public Guid OriginatorId { get; private set; }
    public Guid OwnerId { get; private set; }
    public string CycleIdentifier { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public FiscalAccountStatus Status { get; private set; }

    private readonly List<Folio> _folios = new();
    public IReadOnlyCollection<Folio> Folios => _folios.AsReadOnly();

#pragma warning disable CS8618
    public FiscalAccount() { }
#pragma warning restore CS8618

    private FiscalAccount(
        Guid id,
        Guid originatorId,
        Guid ownerId,
        string cycleIdentifier,
        DateTime createdAt)
    {
        Id = id;
        OriginatorId = originatorId;
        OwnerId = ownerId;
        CycleIdentifier = cycleIdentifier;
        CreatedAt = createdAt;
        Status = FiscalAccountStatus.Open;
    }

    public static FiscalAccount Create(Guid originatorId, Guid ownerId, string cycleIdentifier)
    {
        if (string.IsNullOrWhiteSpace(cycleIdentifier))
        {
            throw new ArgumentException("Cycle identifier is required.", nameof(cycleIdentifier));
        }

        var account = new FiscalAccount(
            Guid.NewGuid(),
            originatorId,
            ownerId,
            cycleIdentifier,
            DateTime.UtcNow);

        account.AddFolio();

        return account;
    }

    public Folio OpenFolio()
    {
        return AddFolio();
    }

    public FolioItem AddFolioItem(
        Guid folioId,
        string description,
        int quantity,
        decimal amount,
        Guid transactionCodeId,
        FolioItemType transactionType,
        DateOnly businessDate)
    {
        var folio = GetFolio(folioId);

        return folio.AddItem(
            description,
            quantity,
            amount,
            transactionCodeId,
            transactionType,
            businessDate);
    }

    public void SettleFolio(Guid folioId)
    {
        GetFolio(folioId).Settle();
    }

    public void CheckOut()
    {
        if (Status == FiscalAccountStatus.CheckedOut)
        {
            throw new FiscalAccountAlreadyCheckedOutException();
        }

        if (_folios.Any(f => f.Status != FolioStatus.Settled))
        {
            throw new FiscalAccountNotSettledException();
        }

        Status = FiscalAccountStatus.CheckedOut;
    }

    private Folio AddFolio()
    {
        var folio = Folio.Create(Id);
        _folios.Add(folio);
        return folio;
    }

    private Folio GetFolio(Guid folioId)
    {
        var folio = _folios.FirstOrDefault(f => f.Id == folioId);

        if (folio is null)
        {
            throw new FolioNotFoundException(folioId);
        }

        return folio;
    }
}