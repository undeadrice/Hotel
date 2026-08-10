using Hotel.Domain.FiscalAccounting.Entities;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Domain.RatePlans.Services;
using Hotel.Domain.Reservations.Entities;
using Hotel.Domain.Reservations.Exceptions;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Reservations.Commands;

public class CreateReservationCommandHandler(
    IReservationRepository reservationRepository,
    IFiscalAccountRepository fiscalAccountRepository,
    IRoomRepository roomRepository,
    IRatePlanRepository ratePlanRepository,
    IRoomAvailabilityService roomAvailabilityService)
    : IRequestHandler<CreateReservationCommand, Guid>
{
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(request.RoomId, cancellationToken);

        if (!room.IsActive)
        {
            throw new RoomNotActiveException();
        }

        var ratePlan = await ratePlanRepository.GetById(request.RatePlanId, cancellationToken);

        if (request.StartDate < ratePlan.StartDate || request.EndDate > ratePlan.EndDate)
        {
            throw RatePlanInvalidForRoomException.DateRangeMismatch();
        }

        var isRoomInRatePlan = ratePlan.Rooms.Any(rpr => rpr.RoomTypeId == room.RoomTypeId);
        if (!isRoomInRatePlan)
        {
            throw RatePlanInvalidForRoomException.RoomNotInRatePlan();
        }

        var reservation = await Reservation.Create(
            request.CreatorId,
            request.RoomId,
            request.RatePlanId,
            request.StartDate,
            request.EndDate,
            request.ArrivalTime,
            request.GuestIds,
            roomAvailabilityService,
            cancellationToken);

        await reservationRepository.Add(reservation, cancellationToken);

        var fiscalAccount = FiscalAccount.Create(reservation.Id, request.CreatorId);

        await fiscalAccountRepository.Add(fiscalAccount, cancellationToken);

        return reservation.Id;
    }
}
