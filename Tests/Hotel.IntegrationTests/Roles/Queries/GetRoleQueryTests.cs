using FluentAssertions;
using Hotel.Application.Roles.Dtos;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Roles.Queries;

public class GetRoleQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRoleQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRole_WithExistingRole_ReturnsRole()
    {
        // Arrange
        var name = $"Manager {Guid.NewGuid():N}";
        var roleId = await RoleTestData.CreateRoleAsync(
            _client,
            name,
            ["RoleView", "RoleEdit"]);

        // Act
        var response = await _client.GetAsync($"/api/roles/{roleId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var role = await response.Content.ReadFromJsonAsync<RoleDto>();
        role.Should().NotBeNull();
        role!.Id.Should().Be(roleId);
        role.Name.Should().Be(name);
        role.Permissions.Should().BeEquivalentTo(["RoleView", "RoleEdit"]);
    }

    [Fact]
    public async Task GetRole_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/roles/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}