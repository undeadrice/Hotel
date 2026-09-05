using Hotel.Domain.NumberCycles.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.NumberCycles.Commands;

[CheckPermission(Permission.NumberCycleDelete)]
public record DeleteNumberCycleCommand(Guid Id) : ICommand;

internal class DeleteNumberCycleCommandHandler(INumberCycleService numberCycleService)
    : IRequestHandler<DeleteNumberCycleCommand>
{
    public async Task Handle(DeleteNumberCycleCommand request, CancellationToken cancellationToken)
    {
        await numberCycleService.Delete(request.Id, cancellationToken);
    }
}
