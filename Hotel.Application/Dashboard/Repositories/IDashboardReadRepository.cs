using Hotel.Application.Dashboard.TransferObjects;

namespace Hotel.Application.Dashboard.Repositories;

public interface IDashboardReadRepository
{
    Task<DashboardDto> GetDashboard(DateOnly businessDate, CancellationToken cancellationToken);
}