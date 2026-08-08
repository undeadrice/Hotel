namespace Hotel.Domain.Folios.Entities;

public class FolioItem
{
    public Guid Id { get; private set; }
    public Guid FolioId { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }

#pragma warning disable CS8618
    public FolioItem() { }
#pragma warning restore CS8618

    private FolioItem(
        Guid id,
        Guid folioId,
        string description,
        decimal amount,
        DateTime createdAt)
    {
        Id = id;
        FolioId = folioId;
        Description = description;
        Amount = amount;
        CreatedAt = createdAt;
    }

    public static FolioItem Create(Guid folioId, string description, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description is required.");
        }

        return new FolioItem(
            Guid.NewGuid(),
            folioId,
            description,
            amount,
            DateTime.UtcNow);
    }
}