using FluentAssertions;
using Hotel.Application.Roles.Commands;
using Hotel.Application.Roles.Dtos;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Roles.Commands;

public class UpdateRoleCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public UpdateRoleCommandTests(HotelWebApplicationFactory factory)
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
    public async Task UpdateRole_WithValidCommand_ReturnsOkAndUpdatesRole()
    {
        // Arrange
        var roleId = await RoleTestData.CreateRoleAsync(_client);
        var updatedName = $"Updated Role {Guid.NewGuid():N}";
        var command = new UpdateRoleCommand(
            roleId,
            updatedName,
            new[] { "RoleView", "RoleEdit" });

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await _client.GetAsync($"/api/roles/{roleId}");
        var role = await getResponse.Content.ReadFromJsonAsync<RoleDto>();
        role.Should().NotBeNull();
        role!.Name.Should().Be(updatedName);
        role.Permissions.Should().BeEquivalentTo(new[] { "RoleView", "RoleEdit" });
    }

    [Fact]
    public async Task UpdateRole_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var command = new UpdateRoleCommand(
            Guid.NewGuid(),
            "Updated Role",
            new[] { "RoleView" });

        // Act
        var response = await _client.PutAsJsonAsync("/api/roles/update", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}