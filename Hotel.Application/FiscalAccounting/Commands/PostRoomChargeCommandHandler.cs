using Hotel.Application.Configurations.Services;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Domain.RatePlans.Services;
using Hotel.Domain.Reservations.Exceptions;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Commands;

public class PostRoomChargeCommandHandler(
    IReservationRepository reservationRepository,
    IRatePlanRepository ratePlanRepository,
    IRoomRepository roomRepository,
    IFiscalAccountRepository fiscalAccountRepository,
    IBusinessDateProvider businessDateProvider)
    : IRequestHandler<PostRoomChargeCommand, Guid>
{
    public async Task<Guid> Handle(PostRoomChargeCommand request, CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.GetById(request.ReservationId, cancellationToken);

        var ratePlan = await ratePlanRepository.GetById(reservation.RatePlanId, cancellationToken);
        var room = await roomRepository.GetById(reservation.RoomId, cancellationToken);

        var ratePlanRoom = ratePlan.Rooms.FirstOrDefault(rpr => rpr.RoomTypeId == room.RoomTypeId);

        if (ratePlanRoom is null)
        {
            throw RatePlanInvalidForRoomException.RoomNotInRatePlan();
        }

        var fiscalAccount = await fiscalAccountRepository.GetByOriginatorId(reservation.Id, cancellationToken);

        var businessDate = await businessDateProvider.GetCurrentBusinessDate(cancellationToken);

        var item = fiscalAccount.PostChargeToMainFolio(
            "Room charge",
            ratePlanRoom.Price,
            ratePlan.TransactionCodeId,
            businessDate);

        return item.Id;
    }
}