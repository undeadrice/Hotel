using Hotel.Domain.RatePlans.Exceptions;

namespace Hotel.Domain.RatePlans.Entities;

public class RatePlan
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Guid TransactionCodeId { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public bool IsActive { get; private set; }

    private readonly List<RatePlanRoom> _rooms = new();

    public IReadOnlyCollection<RatePlanRoom> Rooms => _rooms.AsReadOnly();

#pragma warning disable CS8618
    public RatePlan() { }
#pragma warning restore CS8618

    private RatePlan(Guid id, string name, Guid transactionCodeId, DateOnly startDate, DateOnly endDate)
    {
        Id = id;
        Name = name;
        TransactionCodeId = transactionCodeId;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = true;
    }

    public static RatePlan Create(
        string name,
        Guid transactionCodeId,
        DateOnly startDate,
        DateOnly endDate,
        DateOnly businessDate,
        IEnumerable<RoomTypePriceDefinition> rooms)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new RatePlanNameRequiredException();
        }

        if (transactionCodeId == Guid.Empty)
        {
            throw new RatePlanTransactionCodeRequiredException();
        }

        if (startDate < businessDate)
        {
            throw new RatePlanStartDateInvalidException();
        }

        if (endDate <= startDate)
        {
            throw new RatePlanDateRangeInvalidException();
        }

        var roomList = rooms.ToList();

        if (roomList.Count == 0)
        {
            throw new RatePlanRoomsRequiredException();
        }

        var ratePlan = new RatePlan(Guid.NewGuid(), name, transactionCodeId, startDate, endDate);

        foreach (var room in roomList)
        {
            if (room.RoomTypeId == Guid.Empty)
            {
                throw new RatePlanRoomTypeRequiredException();
            }

            ratePlan.AddRoom(room.RoomTypeId, room.Price);
        }

        return ratePlan;
    }

    public void Update(
        string name,
        Guid transactionCodeId,
        DateOnly startDate,
        DateOnly endDate,
        DateOnly businessDate,
        IEnumerable<RoomTypePriceDefinition> rooms)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new RatePlanNameRequiredException();
        }

        if (transactionCodeId == Guid.Empty)
        {
            throw new RatePlanTransactionCodeRequiredException();
        }

        if (startDate < businessDate)
        {
            throw new RatePlanStartDateInvalidException();
        }

        if (endDate <= startDate)
        {
            throw new RatePlanDateRangeInvalidException();
        }

        var roomList = rooms.ToList();

        if (roomList.Count == 0)
        {
            throw new RatePlanRoomsRequiredException();
        }

        Name = name;
        TransactionCodeId = transactionCodeId;
        StartDate = startDate;
        EndDate = endDate;

        _rooms.Clear();
        foreach (var room in roomList)
        {
            if (room.RoomTypeId == Guid.Empty)
            {
                throw new RatePlanRoomTypeRequiredException();
            }

            AddRoom(room.RoomTypeId, room.Price);
        }
    }

    private void AddRoom(Guid roomTypeId, decimal price)
    {
        var room = RatePlanRoom.Create(Id, roomTypeId, price);
        _rooms.Add(room);
    }
}

public record RoomTypePriceDefinition(Guid RoomTypeId, decimal Price);
