using MediatR;
using Hotel.Domain.Guests.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Commands;

[CheckPermission(Permission.GuestEdit)]
public record UpdateGuestCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber) : ICommand;

internal class UpdateGuestCommandHandler(IGuestRepository guestRepository)
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
    }
}
