using Hotel.Application.Reservations.TransferObjects;
using MediatR;
using Hotel.Application.Reservations.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Reservations.Queries;

[CheckPermission(Permission.ReservationView)]
public record GetReservationByIdQuery(Guid Id) : IRequest<ReservationDto>;

internal class GetReservationByIdQueryHandler(IReservationReadRepository reservationReadRepository)
    : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetById(request.Id, cancellationToken);
    }
}
