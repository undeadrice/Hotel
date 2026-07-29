using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Entities;

public class RoomType
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public decimal BaseRate { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    private RoomType(Guid id, string name, decimal baseRate, string? description)
    {
        Id = id;
        Name = name;
        BaseRate = baseRate;
        Description = description;
        IsActive = true;
    }

#pragma warning disable CS8618
    public RoomType() { }
#pragma warning restore CS8618

    public static RoomType Create(string name, decimal baseRate, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new RoomTypeNameRequiredException();
        }

        if (baseRate <= 0)
        {
            throw new RoomTypeBaseRateInvalidException();
        }

        return new RoomType(Guid.NewGuid(), name, baseRate, description);
    }

    public void Update(string name, decimal baseRate, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new RoomTypeNameRequiredException();
        }

        if (baseRate <= 0)
        {
            throw new RoomTypeBaseRateInvalidException();
        }

        Name = name;
        BaseRate = baseRate;
        Description = description;
    }
}