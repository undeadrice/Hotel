using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Entities;

public class RoomType
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    private RoomType(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = true;
    }

#pragma warning disable CS8618
    public RoomType() { }
#pragma warning restore CS8618

    public static RoomType Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new RoomTypeNameRequiredException();
        }

        return new RoomType(Guid.NewGuid(), name, description);
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new RoomTypeNameRequiredException();
        }

        Name = name;
        Description = description;
    }
}
