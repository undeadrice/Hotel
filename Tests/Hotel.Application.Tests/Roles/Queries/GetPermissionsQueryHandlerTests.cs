using FluentAssertions;
using Hotel.Application.Roles.Queries;
using Xunit;

namespace Hotel.Application.Tests.Roles.Queries;

public class GetPermissionsQueryHandlerTests
{
    private readonly GetPermissionsQueryHandler _handler = new();

    [Fact]
    public async Task Handle_RoleGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Role").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "RoleCreate", "RoleEdit", "RoleDelete", "RoleView" });
    }

    [Fact]
    public async Task Handle_UserGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "User").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "UserCreate", "UserEdit", "UserDelete", "UserView" });
    }

    [Fact]
    public async Task Handle_PermissionsGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Permissions").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "PermissionView" });
    }

    [Fact]
    public async Task Handle_ReservationGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Reservation").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "ReservationCreate", "ReservationEdit", "ReservationView" });
    }

    [Fact]
    public async Task Handle_RoomGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Room").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "RoomCreate", "RoomEdit", "RoomDelete", "RoomView" });
    }

    [Fact]
    public async Task Handle_RoomTypeGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "RoomType").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "RoomTypeCreate", "RoomTypeEdit", "RoomTypeDelete", "RoomTypeView" });
    }

    [Fact]
    public async Task Handle_RatePlanGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "RatePlan").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "RatePlanCreate", "RatePlanEdit", "RatePlanDelete", "RatePlanView" });
    }

    [Fact]
    public async Task Handle_GuestGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Guest").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "GuestCreate", "GuestEdit", "GuestDelete", "GuestView" });
    }

    [Fact]
    public async Task Handle_NumberCycleGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "NumberCycle").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "NumberCycleCreate", "NumberCycleDelete", "NumberCycleView" });
    }

    [Fact]
    public async Task Handle_FiscalAccountGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "FiscalAccount").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "FiscalAccountEdit", "FiscalAccountView" });
    }

    [Fact]
    public async Task Handle_ConfigurationGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Configuration").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "ConfigurationView", "ConfigurationEdit" });
    }

    [Fact]
    public async Task Handle_TransactionCodeGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "TransactionCode").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "TransactionCodeCreate", "TransactionCodeEdit", "TransactionCodeView" });
    }

    [Fact]
    public async Task Handle_TransactionGroupGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "TransactionGroup").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "TransactionGroupCreate", "TransactionGroupEdit", "TransactionGroupView" });
    }

    [Fact]
    public async Task Handle_DashboardGroup_ShouldContainExpectedPermissions()
    {
        // Arrange
        var query = new GetPermissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        var group = result.Should().ContainSingle(g => g.GroupName == "Dashboard").Subject;
        group.Permissions.Should().BeEquivalentTo(
            new[] { "DashboardView" });
    }
}