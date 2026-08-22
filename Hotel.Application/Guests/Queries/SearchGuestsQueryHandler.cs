using Hotel.Application.Guests.TransferObjects;
using MediatR;
using Hotel.Application.Guests.Repositories;

namespace Hotel.Application.Guests.Queries;

internal class SearchGuestsQueryHandler(IGuestReadRepository guestReadRepository)
    : IRequestHandler<SearchGuestsQuery, IReadOnlyCollection<GuestListDto>>
{
    public async Task<IReadOnlyCollection<GuestListDto>> Handle(SearchGuestsQuery request, CancellationToken cancellationToken)
    {
        return await guestReadRepository.Search(
            request.Name,
            request.Phone,
            request.Email,
            request.DocumentNumber,
            cancellationToken);
    }
}