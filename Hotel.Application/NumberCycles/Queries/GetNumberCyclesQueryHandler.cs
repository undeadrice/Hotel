using Hotel.Application.NumberCycles.TransferObjects;
using MediatR;
using Hotel.Application.NumberCycles.Repositories;

namespace Hotel.Application.NumberCycles.Queries;

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