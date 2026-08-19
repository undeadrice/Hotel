using Hotel.Domain.Configurations.Services;
using Hotel.Domain.Reservations.Services;
using MediatR;

namespace Hotel.Application.Configurations.Commands;

public class PerformEndOfDayCommandHandler(
    IConfigurationRepository configurationRepository,
    IReservationRepository reservationRepository)
    : IRequestHandler<PerformEndOfDayCommand, DateOnly>
{
    public async Task<DateOnly> Handle(PerformEndOfDayCommand request, CancellationToken cancellationToken)
    {
        var configuration = await configurationRepository.Get(cancellationToken);

        configuration.EndOfDay();

        var businessDate = configuration.CurrentBusinessDate;

        var reservations = await reservationRepository.GetForEndOfDay(businessDate, cancellationToken);

        foreach (var reservation in reservations)
        {
            reservation.TransitionOnEndOfDay(businessDate);
        }

        return businessDate;
    }
}