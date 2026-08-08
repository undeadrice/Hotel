using Hotel.Domain.Guests;
using Hotel.Domain.Guests.Services;
using MediatR;

namespace Hotel.Application.Guests.Commands;

public class CreateGuestCommandHandler(IGuestRepository guestRepository) : IRequestHandler<CreateGuestCommand, Guid>
{
    public async Task<Guid> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = Guest.Create(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.DocumentNumber);

        await guestRepository.Add(guest, cancellationToken);

        return guest.Id;
    }
}