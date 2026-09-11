using Hotel.Application.Reservations.TransferObjects;
using MediatR;
using Hotel.Application.Reservations.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Reservations.Queries;

[CheckPermission(Permission.ReservationView)]
public record GetReservationsQuery : IRequest<IReadOnlyCollection<ReservationListDto>>;

internal class GetReservationsQueryHandler(IReservationReadRepository reservationReadRepository)
    : IRequestHandler<GetReservationsQuery, IReadOnlyCollection<ReservationListDto>>
{
    public async Task<IReadOnlyCollection<ReservationListDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetAll(cancellationToken);
    }
}
