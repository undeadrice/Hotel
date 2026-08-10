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
    IRatePlanRepository ratePlanRepository)
    : IRequestHandler<CreateReservationCommand, Guid>
{
    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var room = await roomRepository.GetById(request.RoomId, cancellationToken);

        if (!room.IsActive)
        {
            throw new RoomNotActiveException();
        }

        var overlappingReservations = await reservationRepository.GetAll(
            cancellationToken,
            r => r.RoomId == request.RoomId
                 && r.StartDate < request.EndDate
                 && r.EndDate > request.StartDate);

        if (overlappingReservations.Count != 0)
        {
            throw new RoomNotAvailableException();
        }

        var ratePlan = await ratePlanRepository.GetById(request.RatePlanId, cancellationToken);

        var reservationStartDate = DateOnly.FromDateTime(request.StartDate);
        var reservationEndDate = DateOnly.FromDateTime(request.EndDate);

        if (reservationStartDate < ratePlan.StartDate || reservationEndDate > ratePlan.EndDate)
        {
            throw RatePlanInvalidForRoomException.DateRangeMismatch();
        }

        var isRoomInRatePlan = ratePlan.Rooms.Any(rpr => rpr.RoomTypeId == room.RoomTypeId);
        if (!isRoomInRatePlan)
        {
            throw RatePlanInvalidForRoomException.RoomNotInRatePlan();
        }

        var reservation = Reservation.Create(
            request.CreatorId,
            request.RoomId,
            request.RatePlanId,
            request.StartDate,
            request.EndDate,
            request.ArrivalTime,
            request.GuestIds);

        await reservationRepository.Add(reservation, cancellationToken);

        var fiscalAccount = FiscalAccount.Create(reservation.Id, request.CreatorId);

        await fiscalAccountRepository.Add(fiscalAccount, cancellationToken);

        return reservation.Id;
    }
}
