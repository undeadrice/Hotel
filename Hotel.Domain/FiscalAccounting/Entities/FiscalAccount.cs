namespace Hotel.Domain.FiscalAccounting.Entities;

public class FiscalAccount
{
    public Guid Id { get; private set; }
    public Guid OriginatorId { get; private set; }
    public Guid OwnerId { get; private set; }
    public string CycleIdentifier { get; private set; }
    public DateTime CreatedAt { get; private set; }

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

    private Folio AddFolio()
    {
        var folio = Folio.Create(Id);
        _folios.Add(folio);
        return folio;
    }
}