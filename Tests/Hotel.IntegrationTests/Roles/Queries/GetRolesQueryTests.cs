using FluentAssertions;
using Hotel.Application.Roles.Dtos;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.Roles.Queries;

public class GetRolesQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetRolesQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetRoles_AfterCreatingRole_ReturnsRole()
    {
        // Arrange
        var name = $"Housekeeping {Guid.NewGuid():N}";
        var roleId = await RoleTestData.CreateRoleAsync(_client, name);

        // Act
        var response = await _client.GetAsync("/api/roles");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roles = await response.Content.ReadFromJsonAsync<List<RoleSimpleDto>>();
        roles.Should().ContainSingle(r => r.Name == name);

        var role = roles!.Single(r => r.Name == name);
        role.Id.Should().Be(roleId);
    }
}