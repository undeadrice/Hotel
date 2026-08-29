using Hotel.Application.Guests.TransferObjects;
using MediatR;
using Hotel.Application.Guests.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Queries;

[CheckPermission(Permission.GuestView)]
public record GetGuestByIdQuery(Guid Id) : IRequest<GuestDto>;

internal class GetGuestByIdQueryHandler(IGuestReadRepository guestReadRepository)
    : IRequestHandler<GetGuestByIdQuery, GuestDto>
{
    public async Task<GuestDto> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
    {
        return await guestReadRepository.GetById(request.Id, cancellationToken);
    }
}
