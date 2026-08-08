using Hotel.Application.Dashboard.TransferObjects;
using MediatR;

namespace Hotel.Application.Dashboard.Queries;

public record GetDashboardQuery() : IRequest<DashboardDto>;