using Hotel.Application.Reservations.Services;
using Hotel.Application.Reservations.TransferObjects;
using MediatR;

namespace Hotel.Application.Reservations.Queries;

internal class GetReservationByIdQueryHandler(IReservationReadRepository reservationReadRepository)
    : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        return await reservationReadRepository.GetById(request.Id, cancellationToken);
    }
}