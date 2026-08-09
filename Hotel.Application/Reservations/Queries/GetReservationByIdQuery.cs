using Hotel.Application.Reservations.TransferObjects;
using MediatR;

namespace Hotel.Application.Reservations.Queries;

public record GetReservationByIdQuery(Guid Id) : IRequest<ReservationDto>;