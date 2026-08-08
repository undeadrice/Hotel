namespace Hotel.Domain.RatePlans.Entities;

public class RatePlanRoom
{
    public Guid RatePlanId { get; private set; }

    public Guid RoomTypeId { get; private set; }

    public decimal Price { get; private set; }

#pragma warning disable CS8618
    public RatePlanRoom() { }
#pragma warning restore CS8618

    public RatePlanRoom(Guid roomTypeId, decimal price)
    {
        RoomTypeId = roomTypeId;
        Price = price;
    }

    internal void SetRatePlanId(Guid ratePlanId)
    {
        RatePlanId = ratePlanId;
    }
}