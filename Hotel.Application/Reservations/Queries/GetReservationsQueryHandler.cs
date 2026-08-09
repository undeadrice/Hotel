using Hotel.Application.Reservations.Services;
using Hotel.Application.Reservations.TransferObjects;
using MediatR;

namespace Hotel.Application.Reservations.Queries;

internal class GetReservationsQueryHandler(IReservationReadRepository reservationReadRepository)
    : IRequestHandler<GetReservationsQuery, IReadOnlyCollection<ReservationListDto>>
{
    public async Task<IReadOnlyCollection<ReservationListDto>> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetAll(cancellationToken);
    }
}