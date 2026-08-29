using Hotel.Application.Common;
using Hotel.Domain.Reservations.Exceptions;
using MediatR;
using Hotel.Domain.RatePlans.Repositories;
using Hotel.Domain.Reservations.Repositories;
using Hotel.Domain.Rooming.Repositories;
using Hotel.Domain.Configurations.Repositories;
using Hotel.Domain.FiscalAccounting.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Configurations.Commands;

[CheckPermission(Permission.ConfigurationEdit)]
public record PerformEndOfDayCommand : ICommand<DateOnly>;

public class PerformEndOfDayCommandHandler(
    IConfigurationRepository configurationRepository,
    IReservationRepository reservationRepository,
    IFiscalAccountRepository fiscalAccountRepository,
    IRoomRepository roomRepository,
    IRatePlanRepository ratePlanRepository,
    IDateTimeProvider dateTimeProvider)
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
                businessDate,
                dateTimeProvider.UtcNow);
        }
    }
}