using Hotel.Application.Guests.TransferObjects;
using MediatR;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Queries;

[CheckPermission(Permission.GuestView)]
public record GetGuestsQuery() : IRequest<IReadOnlyCollection<GuestListDto>>;

internal class GetGuestsQueryHandler(IGuestReadRepository guestReadRepository)
    : IRequestHandler<GetGuestsQuery, IReadOnlyCollection<GuestListDto>>
{
    public async Task<IReadOnlyCollection<GuestListDto>> Handle(GetGuestsQuery request, CancellationToken cancellationToken)
    {
        return await guestReadRepository.GetAll(cancellationToken);
    }
}
