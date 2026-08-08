using Hotel.Domain.Transactions.Enums;
using Hotel.Domain.Transactions.Exceptions;

namespace Hotel.Domain.Transactions.Entities;

public class TransactionGroup
{
    private readonly List<TransactionCode> _transactionCodes = [];

    public Guid Id { get; private set; }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public TransactionType Type { get; private set; }

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<TransactionCode> TransactionCodes => _transactionCodes.AsReadOnly();

#pragma warning disable CS8618
    public TransactionGroup() { }
#pragma warning restore CS8618

    private TransactionGroup(
        Guid id,
        string code,
        string name,
        string? description,
        TransactionType type)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Type = type;
        IsActive = true;
    }

    public static TransactionGroup Create(
        string code,
        string name,
        string? description,
        TransactionType type)
    {
        Validate(code, name);

        return new TransactionGroup(
            Guid.NewGuid(),
            code.Trim().ToUpperInvariant(),
            name.Trim(),
            description,
            type);
    }

    public void Update(
        string code,
        string name,
        string? description,
        TransactionType type)
    {
        Validate(code, name);

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        Description = description;
        Type = type;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static void Validate(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new TransactionGroupCodeRequiredException();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new TransactionGroupNameRequiredException();
        }
    }
}
