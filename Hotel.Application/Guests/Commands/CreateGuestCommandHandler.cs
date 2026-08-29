using Hotel.Domain.Guests;
using MediatR;
using Hotel.Domain.Guests.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Guests.Commands;

[CheckPermission(Permission.GuestCreate)]
public record CreateGuestCommand(
    string FirstName,
    string LastName,
    string Phone,
    string Email,
    string DocumentNumber) : ICommand<Guid>;
internal class CreateGuestCommandHandler(IGuestRepository guestRepository) : IRequestHandler<CreateGuestCommand, Guid>
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
