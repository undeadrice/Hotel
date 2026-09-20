using Hotel.Application.NumberCycles.TransferObjects;
using MediatR;
using Hotel.Application.NumberCycles.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.NumberCycles.Queries;

[CheckPermission(Permission.NumberCycleView)]
public record GetNumberCycleByIdQuery(Guid Id) : IRequest<NumberCycleDto>;

internal class GetNumberCycleByIdQueryHandler(INumberCycleReadRepository numberCycleReadRepository)
    : IRequestHandler<GetNumberCycleByIdQuery, NumberCycleDto>
{
    public async Task<NumberCycleDto> Handle(
        GetNumberCycleByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await numberCycleReadRepository.GetById(request.Id, cancellationToken);
    }
}
