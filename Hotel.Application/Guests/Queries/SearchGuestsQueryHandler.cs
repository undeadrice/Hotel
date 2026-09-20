using Hotel.Application.Guests.TransferObjects;
using MediatR;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Queries;

[CheckPermission(Permission.GuestView)]
public record SearchGuestsQuery(
    string? Name,
    string? Phone,
    string? Email,
    string? DocumentNumber)
    : IRequest<IReadOnlyCollection<GuestListDto>>;

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
