using Hotel.Application.Dashboard.Services;
using Hotel.Application.Dashboard.TransferObjects;
using MediatR;

namespace Hotel.Application.Dashboard.Queries;

internal class GetDashboardQueryHandler(IDashboardReadRepository dashboardReadRepository)
    : IRequestHandler<GetDashboardQuery, DashboardDto>
{
    public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        return await dashboardReadRepository.GetDashboard(cancellationToken);
    }
}