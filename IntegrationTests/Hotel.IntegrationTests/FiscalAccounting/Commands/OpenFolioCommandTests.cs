using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Commands;

public class OpenFolioCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public OpenFolioCommandTests(HotelWebApplicationFactory factory)
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
    public async Task OpenFolio_WithValidAccount_ReturnsFolioId()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios",
            new OpenFolioCommand(context.FiscalAccountId));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var folioId = await response.Content.ReadFromJsonAsync<Guid>();
        folioId.Should().NotBeEmpty();

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        account.Folios.Should().HaveCount(2);

        var openedFolio = account.Folios.Should().Contain(f => f.Id == folioId).Which;
        openedFolio.IsMainFolio.Should().BeFalse();
    }

    [Fact]
    public async Task OpenFolio_WithNonExistentAccount_ReturnsNotFound()
    {
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/folios",
            new OpenFolioCommand(Guid.NewGuid()));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}