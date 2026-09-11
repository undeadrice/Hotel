using Hotel.Application.Configurations.Services;
using Hotel.Application.Dashboard.TransferObjects;
using MediatR;
using Hotel.Application.Dashboard.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Dashboard.Queries;

[CheckPermission(Permission.DashboardView)]
public record GetDashboardQuery() : IRequest<DashboardDto>;

internal class GetDashboardQueryHandler(
    IDashboardReadRepository dashboardReadRepository,
    IBusinessDateProvider businessDateProvider)
    : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var businessDate = await businessDateProvider.GetCurrentBusinessDate(cancellationToken);

        return await dashboardReadRepository.GetDashboard(businessDate, cancellationToken);
    }
}
