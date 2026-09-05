using Hotel.Application.NumberCycles.TransferObjects;
using MediatR;
using Hotel.Application.NumberCycles.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.NumberCycles.Queries;

[CheckPermission(Permission.NumberCycleView)]
public record GetNumberCyclesQuery : IRequest<IReadOnlyCollection<NumberCycleDto>>;

internal class GetNumberCyclesQueryHandler(INumberCycleReadRepository numberCycleReadRepository)
    : IRequestHandler<GetNumberCyclesQuery, IReadOnlyCollection<NumberCycleDto>>
{
    public async Task<IReadOnlyCollection<NumberCycleDto>> Handle(
        GetNumberCyclesQuery request,
        CancellationToken cancellationToken)
    {
        return await numberCycleReadRepository.GetAll(cancellationToken);
    }
}
