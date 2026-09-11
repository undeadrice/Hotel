using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Entities;

public class Room
{
    public Guid Id { get; private set; }

    public string RoomNumber { get; private set; }

    public Guid RoomTypeId { get; private set; }

    public bool IsActive { get; private set; }

    private Room(Guid id, string roomNumber, Guid roomTypeId)
    {
        Id = id;
        RoomNumber = roomNumber;
        RoomTypeId = roomTypeId;
        IsActive = true;
    }

#pragma warning disable CS8618
    public Room() { }
#pragma warning restore CS8618

    internal static Room Create(string roomNumber, Guid roomTypeId)
    {
        if (string.IsNullOrWhiteSpace(roomNumber))
        {
            throw new RoomNumberRequiredException();
        }

        return new Room(Guid.NewGuid(), roomNumber, roomTypeId);
    }

    internal void UpdateRoomNumber(string roomNumber)
    {
        if (string.IsNullOrWhiteSpace(roomNumber))
        {
            throw new RoomNumberRequiredException();
        }

        RoomNumber = roomNumber;
    }

    internal void ChangeRoomType(Guid roomTypeId)
    {
        RoomTypeId = roomTypeId;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new RoomStatusChangeInvalidException("Room is already deactivated.");
        }

        IsActive = false;
    }
}
