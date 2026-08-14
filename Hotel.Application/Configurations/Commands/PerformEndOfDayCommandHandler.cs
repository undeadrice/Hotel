using Hotel.Domain.Configurations.Services;
using MediatR;

namespace Hotel.Application.Configurations.Commands;

public class PerformEndOfDayCommandHandler(IConfigurationRepository configurationRepository)
    : IRequestHandler<PerformEndOfDayCommand, DateOnly>
{
    public async Task<DateOnly> Handle(PerformEndOfDayCommand request, CancellationToken cancellationToken)
    {
        var configuration = await configurationRepository.Get(cancellationToken);

        configuration.EndOfDay();

        return configuration.CurrentBusinessDate;
    }
}