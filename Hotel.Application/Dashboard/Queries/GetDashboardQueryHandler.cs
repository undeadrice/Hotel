using Hotel.Application.Configurations.Services;
using Hotel.Application.Dashboard.TransferObjects;
using MediatR;
using Hotel.Application.Dashboard.Repositories;

namespace Hotel.Application.Dashboard.Queries;

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