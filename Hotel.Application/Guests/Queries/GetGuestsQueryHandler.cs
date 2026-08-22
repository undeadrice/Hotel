using Hotel.Application.Guests.TransferObjects;
using MediatR;
using Hotel.Application.Guests.Repositories;

namespace Hotel.Application.Guests.Queries;

internal class GetGuestsQueryHandler(IGuestReadRepository guestReadRepository)
    : IRequestHandler<GetGuestsQuery, IReadOnlyCollection<GuestListDto>>
{
    public async Task<IReadOnlyCollection<GuestListDto>> Handle(GetGuestsQuery request, CancellationToken cancellationToken)
    {
        return await guestReadRepository.GetAll(cancellationToken);
    }
}