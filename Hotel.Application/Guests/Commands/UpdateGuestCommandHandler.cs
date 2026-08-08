using Hotel.Domain.Guests.Services;
using MediatR;

namespace Hotel.Application.Guests.Commands;

public class UpdateGuestCommandHandler(IGuestRepository guestRepository)
    : IRequestHandler<UpdateGuestCommand>
{
    public async Task Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await guestRepository.GetById(request.Id, cancellationToken);
        guest.UpdateProfile(
            request.FirstName,
            request.LastName,
            request.Phone,
            request.Email,
            request.DocumentNumber);

        await guestRepository.Update(guest, cancellationToken);
    }
}