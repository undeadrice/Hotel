using Hotel.Application.NumberCycles.Services;
using Hotel.Application.NumberCycles.TransferObjects;
using MediatR;

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