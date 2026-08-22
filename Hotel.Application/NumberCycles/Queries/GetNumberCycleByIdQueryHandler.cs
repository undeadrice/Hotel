using Hotel.Application.NumberCycles.TransferObjects;
using MediatR;
using Hotel.Application.NumberCycles.Repositories;

namespace Hotel.Application.NumberCycles.Queries;

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