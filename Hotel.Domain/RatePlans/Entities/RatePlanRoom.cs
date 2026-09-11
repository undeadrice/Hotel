using Hotel.Domain.RatePlans.Exceptions;

namespace Hotel.Domain.RatePlans.Entities;

public class RatePlanRoom
{
    public Guid RatePlanId { get; private set; }

    public Guid RoomTypeId { get; private set; }

    public decimal Price { get; private set; }

#pragma warning disable CS8618
    public RatePlanRoom() { }
#pragma warning restore CS8618

    private RatePlanRoom(Guid ratePlanId, Guid roomTypeId, decimal price)
    {
        RatePlanId = ratePlanId;
        RoomTypeId = roomTypeId;
        Price = price;
    }

    public static RatePlanRoom Create(Guid ratePlanId, Guid roomTypeId, decimal price)
    {
        if (price < 0)
        {
            throw new RatePlanPriceInvalidException();
        }

        return new RatePlanRoom(ratePlanId, roomTypeId, price);
    }
}