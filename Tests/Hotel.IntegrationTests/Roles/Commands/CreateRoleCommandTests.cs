using FluentAssertions;
using Hotel.Application.Roles.Commands;
using Hotel.Application.Roles.Dtos;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Roles.Commands;

public class CreateRoleCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateRoleCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateRole_WithValidCommand_ReturnsOkAndCreatesRole()
    {
        // Arrange
        var name = $"Front Desk {Guid.NewGuid():N}";
        var command = new CreateRoleCommand(
            name,
            new[] { "RoleCreate", "RoleView" });

        // Act
        var response = await _client.PostAsJsonAsync("/api/roles", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roles = await _client.GetFromJsonAsync<List<RoleSimpleDto>>("/api/roles");
        var createdRole = roles!.Single(r => r.Name == name);
        createdRole.Id.Should().NotBeEmpty();

        var detailResponse = await _client.GetAsync($"/api/roles/{createdRole.Id}");
        var detail = await detailResponse.Content.ReadFromJsonAsync<RoleDto>();
        detail.Should().NotBeNull();
        detail!.Name.Should().Be(name);
        detail.Permissions.Should().BeEquivalentTo(new[] { "RoleCreate", "RoleView" });
    }
}