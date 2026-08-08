namespace Hotel.Domain.Folios.Entities;

public class Folio
{
    public Guid Id { get; private set; }
    public Guid OriginatorId { get; private set; }
    public Guid GuestId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<FolioItem> _items = new();
    public IReadOnlyCollection<FolioItem> Items => _items.AsReadOnly();

#pragma warning disable CS8618
    public Folio() { }
#pragma warning restore CS8618

    private Folio(
        Guid id,
        Guid originatorId,
        Guid guestId,
        DateTime createdAt)
    {
        Id = id;
        OriginatorId = originatorId;
        GuestId = guestId;
        CreatedAt = createdAt;
    }

    public static Folio Create(Guid originatorId, Guid guestId)
    {
        return new Folio(
            Guid.NewGuid(),
            originatorId,
            guestId,
            DateTime.UtcNow);
    }
}