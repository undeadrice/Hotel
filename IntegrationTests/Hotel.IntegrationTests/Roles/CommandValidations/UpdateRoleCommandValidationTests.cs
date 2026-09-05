using FluentAssertions;
using Hotel.Application.Roles.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Roles.CommandValidations;

public class UpdateRoleCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRoleCommandValidationTests(HotelWebApplicationFactory factory)
    {
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        await _factory.CreateDatabase();
        _client = await _factory.CreateAuthenticatedClientAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.DeleteDatabase();
    }

    [Fact]
    public async Task UpdateRole_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoleCommand(Guid.Empty, "Admin", ["RoleView"]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRole_WithEmptyName_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoleCommand(Guid.NewGuid(), string.Empty, ["RoleView"]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRole_WithNameExceedingMaxLength_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoleCommand(Guid.NewGuid(), new string('a', 257), ["RoleView"]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRole_WithNullPermissions_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoleCommand(Guid.NewGuid(), "Admin", null!);

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateRole_WithInvalidPermission_ReturnsBadRequest()
    {
        // Arrange
        var command = new UpdateRoleCommand(Guid.NewGuid(), "Admin", ["NotARealPermission"]);

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}