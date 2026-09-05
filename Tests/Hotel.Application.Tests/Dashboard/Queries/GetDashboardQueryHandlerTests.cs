using FluentAssertions;
using Hotel.Application.Configurations.Services;
using Hotel.Application.Dashboard.Queries;
using Hotel.Application.Dashboard.Repositories;
using Hotel.Application.Dashboard.TransferObjects;
using NSubstitute;
using Xunit;

namespace Hotel.Application.Tests.Dashboard.Queries;

public class GetDashboardQueryHandlerTests
{
    private readonly IDashboardReadRepository _dashboardReadRepository;
    private readonly IBusinessDateProvider _businessDateProvider;
    private readonly GetDashboardQueryHandler _handler;

    public GetDashboardQueryHandlerTests()
    {
        _dashboardReadRepository = Substitute.For<IDashboardReadRepository>();
        _businessDateProvider = Substitute.For<IBusinessDateProvider>();
        _handler = new GetDashboardQueryHandler(_dashboardReadRepository, _businessDateProvider);
    }

    [Fact]
    public async Task Handle_ShouldCallBusinessDateProviderWithCancellationToken()
    {
        // Arrange
        var cancellationToken = new CancellationToken(canceled: true);
        _businessDateProvider.GetCurrentBusinessDate(cancellationToken).Returns(new DateOnly(2026, 8, 23));

        var query = new GetDashboardQuery();

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        await _businessDateProvider.Received(1).GetCurrentBusinessDate(cancellationToken);
    }

    [Fact]
    public async Task Handle_ShouldCallRepositoryWithBusinessDateAndCancellationToken()
    {
        // Arrange
        var businessDate = new DateOnly(2026, 8, 23);
        var cancellationToken = new CancellationToken(canceled: true);

        _businessDateProvider.GetCurrentBusinessDate(cancellationToken).Returns(businessDate);
        _dashboardReadRepository.GetDashboard(businessDate, cancellationToken).Returns(new DashboardDto(0, 0, 0, 0, 0, businessDate));

        var query = new GetDashboardQuery();

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        await _dashboardReadRepository.Received(1).GetDashboard(businessDate, cancellationToken);
    }

    [Fact]
    public async Task Handle_ShouldReturnDashboardFromRepository()
    {
        // Arrange
        var businessDate = new DateOnly(2026, 8, 23);
        var cancellationToken = CancellationToken.None;

        _businessDateProvider.GetCurrentBusinessDate(cancellationToken).Returns(businessDate);

        var expected = new DashboardDto(
            RoomCount: 50,
            OccupiedRoomCount: 25,
            GuestCount: 30,
            GuestsOnSiteCount: 20,
            OccupancyPercentage: 50.0,
            CurrentBusinessDate: businessDate);

        _dashboardReadRepository.GetDashboard(businessDate, cancellationToken).Returns(expected);

        var query = new GetDashboardQuery();

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().Be(expected);
    }
}