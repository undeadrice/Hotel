using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.CommandValidations;

public class SettleFolioCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public SettleFolioCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task SettleFolio_WithEmptyAccountId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(Guid.Empty, Guid.NewGuid()));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SettleFolio_WithEmptyFolioId_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios/settle",
            new SettleFolioCommand(Guid.NewGuid(), Guid.Empty));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}