using Hotel.Domain.Transactions.Exceptions;

namespace Hotel.Domain.Transactions.Entities;

public class TransactionCode
{
    public Guid Id { get; private set; }

    public Guid TransactionGroupId { get; private set; }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public bool IsActive { get; private set; }

#pragma warning disable CS8618
    public TransactionCode() { }
#pragma warning restore CS8618

    private TransactionCode(
        Guid id,
        Guid transactionGroupId,
        string code,
        string name)
    {
        Id = id;
        TransactionGroupId = transactionGroupId;
        Code = code;
        Name = name;
        IsActive = true;
    }

    public static TransactionCode Create(
        Guid transactionGroupId,
        string code,
        string name)
    {
        Validate(code, name);

        return new TransactionCode(
            Guid.NewGuid(),
            transactionGroupId,
            code.Trim().ToUpperInvariant(),
            name.Trim());
    }

    public void Update(
        string code,
        string name)
    {
        Validate(code, name);

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
    }

    public void ChangeGroup(Guid transactionGroupId)
    {
        TransactionGroupId = transactionGroupId;
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
            throw new TransactionCodeCodeRequiredException();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new TransactionCodeNameRequiredException();
        }
    }
}
