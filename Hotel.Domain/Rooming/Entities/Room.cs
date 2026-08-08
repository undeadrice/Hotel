using Hotel.Domain.Rooming.Enums;
using Hotel.Domain.Rooming.Exceptions;

namespace Hotel.Domain.Rooming.Entities;

public class Room
{
    public Guid Id { get; private set; }

    public string RoomNumber { get; private set; }

    public Guid RoomTypeId { get; private set; }

    public RoomStatus Status { get; private set; }

    public bool IsActive { get; private set; }

    private Room(Guid id, string roomNumber, Guid roomTypeId, RoomStatus status)
    {
        Id = id;
        RoomNumber = roomNumber;
        RoomTypeId = roomTypeId;
        Status = status;
        IsActive = true;
    }

#pragma warning disable CS8618
    public Room() { }
#pragma warning restore CS8618

    public static Room Create(string roomNumber, Guid roomTypeId)
    {
        if (string.IsNullOrWhiteSpace(roomNumber))
        {
            throw new RoomNumberRequiredException();
        }

        return new Room(Guid.NewGuid(), roomNumber, roomTypeId, RoomStatus.Available);
    }

    public void UpdateRoomNumber(string roomNumber)
    {
        if (string.IsNullOrWhiteSpace(roomNumber))
        {
            throw new RoomNumberRequiredException();
        }

        RoomNumber = roomNumber;
    }

    public void ChangeRoomType(Guid roomTypeId)
    {
        RoomTypeId = roomTypeId;
    }

    public void ChangeStatus(RoomStatus newStatus)
    {
        if (Status == newStatus)
        {
            throw new RoomStatusChangeInvalidException("Room is already in this status.");
        }

        if (newStatus == RoomStatus.Available && Status == RoomStatus.Reserved)
        {
            throw new RoomStatusChangeInvalidException(
                "Cannot set room to Available when there is an active reservation for today.");
        }

        Status = newStatus;
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