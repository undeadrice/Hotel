using Hotel.Application.Dashboard.TransferObjects;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Dashboard.Queries;

[CheckPermission(Permission.DashboardView)]
public record GetDashboardQuery() : IRequest<DashboardDto>;
