using Hotel.Application.Reservations.TransferObjects;
using MediatR;
using Hotel.Application.Reservations.Repositories;

namespace Hotel.Application.Reservations.Queries;

internal class GetReservationByIdQueryHandler(IReservationReadRepository reservationReadRepository)
    : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetById(request.Id, cancellationToken);
    }
}