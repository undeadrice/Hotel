using Hotel.Domain.Configurations.Services;
using Hotel.Domain.FiscalAccounting.Services;
using Hotel.Domain.RatePlans.Services;
using Hotel.Domain.Reservations.Exceptions;
using Hotel.Domain.Reservations.Services;
using Hotel.Domain.Rooming.Services;
using MediatR;

namespace Hotel.Application.Configurations.Commands;

public class PerformEndOfDayCommandHandler(
    IConfigurationRepository configurationRepository,
    IReservationRepository reservationRepository,
    IFiscalAccountRepository fiscalAccountRepository,
    IRoomRepository roomRepository,
    IRatePlanRepository ratePlanRepository)
    : IRequestHandler<PerformEndOfDayCommand, DateOnly>
{
    public async Task<DateOnly> Handle(PerformEndOfDayCommand request, CancellationToken cancellationToken)
    {
        var configuration = await configurationRepository.Get(cancellationToken);
        var previousBusinessDate = configuration.CurrentBusinessDate;

        configuration.EndOfDay();

        var businessDate = configuration.CurrentBusinessDate;

        var dueOutReservations = await reservationRepository.GetInHouseEndingOnDate(previousBusinessDate, cancellationToken);

        if (dueOutReservations.Count > 0)
        {
            throw new ReservationDueOutException();
        }

        var reservations = await reservationRepository.GetForEndOfDay(businessDate, cancellationToken);

        foreach (var reservation in reservations)
        {
            reservation.TransitionOnEndOfDay(businessDate);
        }

        await PostRoomChargesForOvernightStay(previousBusinessDate, cancellationToken);

        return businessDate;
    }

    private async Task PostRoomChargesForOvernightStay(DateOnly businessDate, CancellationToken cancellationToken)
    {
        var inHouseReservations = await reservationRepository.GetInHouseForBusinessDate(businessDate, cancellationToken);

        foreach (var reservation in inHouseReservations)
        {
            var ratePlan = await ratePlanRepository.GetById(reservation.RatePlanId, cancellationToken);
            var room = await roomRepository.GetById(reservation.RoomId, cancellationToken);

            var ratePlanRoom = ratePlan.Rooms.FirstOrDefault(rpr => rpr.RoomTypeId == room.RoomTypeId);

            if (ratePlanRoom is null)
            {
                continue;
            }

            var fiscalAccount = await fiscalAccountRepository.GetByOriginatorId(reservation.Id, cancellationToken);

            fiscalAccount.PostChargeToMainFolio(
                "Room charge",
                ratePlanRoom.Price,
                ratePlan.TransactionCodeId,
                businessDate);
        }
    }
}
