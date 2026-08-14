using Hotel.Application.Dashboard.TransferObjects;

namespace Hotel.Application.Dashboard.Services;

public interface IDashboardReadRepository
{
    Task<DashboardDto> GetDashboard(DateOnly businessDate, CancellationToken cancellationToken);
}