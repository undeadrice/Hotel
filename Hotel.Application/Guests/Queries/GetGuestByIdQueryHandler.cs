using Hotel.Application.Guests.TransferObjects;
using MediatR;
using Hotel.Application.Guests.Repositories;

namespace Hotel.Application.Guests.Queries;

internal class GetGuestByIdQueryHandler(IGuestReadRepository guestReadRepository)
    : IRequestHandler<GetGuestByIdQuery, GuestDto>
{
    public async Task<GuestDto> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
    {
        return await guestReadRepository.GetById(request.Id, cancellationToken);
    }
}