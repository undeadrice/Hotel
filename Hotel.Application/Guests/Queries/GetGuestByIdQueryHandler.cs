using Hotel.Application.Guests.Services;
using Hotel.Application.Guests.TransferObjects;
using MediatR;

namespace Hotel.Application.Guests.Queries;

internal class GetGuestByIdQueryHandler(IGuestReadRepository guestReadRepository)
    : IRequestHandler<GetGuestByIdQuery, GuestDto>
{
    public async Task<GuestDto> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
    {
        return await guestReadRepository.GetById(request.Id, cancellationToken);
    }
}