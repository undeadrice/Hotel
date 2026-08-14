using Hotel.Application.NumberCycles.TransferObjects;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.NumberCycles.Queries;

[CheckPermission(Permission.NumberCycleView)]
public record GetNumberCycleByIdQuery(Guid Id) : IRequest<NumberCycleDto>;