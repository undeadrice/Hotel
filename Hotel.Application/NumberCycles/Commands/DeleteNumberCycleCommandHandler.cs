using Hotel.Domain.NumberCycles.Services;
using MediatR;

namespace Hotel.Application.NumberCycles.Commands;

public class DeleteNumberCycleCommandHandler(INumberCycleService numberCycleService)
    : IRequestHandler<DeleteNumberCycleCommand>
{
    public async Task Handle(DeleteNumberCycleCommand request, CancellationToken cancellationToken)
    {
        await numberCycleService.Delete(request.Id, cancellationToken);
    }
}