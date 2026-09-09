using FluentAssertions;
using Hotel.Application.FiscalAccounting.TransferObjects;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Queries;

public class GetFiscalAccountByIdQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetFiscalAccountByIdQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetFiscalAccountById_WithValidId_ReturnsAccount()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        // Act
        var response = await _client.GetAsync($"/api/fiscalaccounts/{context.FiscalAccountId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var account = await response.Content.ReadFromJsonAsync<FiscalAccountDetailsDto>();
        account.Should().NotBeNull();
        account!.Id.Should().Be(context.FiscalAccountId);
        account.OriginatorId.Should().Be(context.ReservationId);
        account.CycleIdentifier.Should().Be("FA-1");
        account.OwnerFullName.Should().Be("John Doe");
        account.Status.Should().Be(FiscalAccountStatus.Open);

        account.Folios.Should().ContainSingle();
        var folio = account.Folios.Single();
        folio.Id.Should().Be(context.MainFolioId);
        folio.IsMainFolio.Should().BeTrue();
        folio.Status.Should().Be(FolioStatus.Open);
        folio.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFiscalAccountById_WithNonExistentId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync($"/api/fiscalaccounts/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}