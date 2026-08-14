using Hotel.Domain.NumberCycles.Services;
using MediatR;

namespace Hotel.Application.NumberCycles.Commands;

public class CreateNumberCycleCommandHandler(INumberCycleService numberCycleService)
    : IRequestHandler<CreateNumberCycleCommand, Guid>
{
    public async Task<Guid> Handle(CreateNumberCycleCommand request, CancellationToken cancellationToken)
    {
        var cycle = await numberCycleService.Create(
            request.Topic,
            request.Prefix,
            request.StartIndex,
            cancellationToken);

        return cycle.Id;
    }
}