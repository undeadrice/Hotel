using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.CommandValidations;

public class CreateFolioItemCommandValidationTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateFolioItemCommandValidationTests(HotelWebApplicationFactory factory)
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
    public async Task CreateFolioItem_WithEmptyFolioId_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            Guid.Empty,
            "Room service",
            1,
            25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFolioItem_WithEmptyDescription_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            string.Empty,
            1,
            25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFolioItem_WithDescriptionLongerThan500_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            new string('a', 501),
            1,
            25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFolioItem_WithZeroQuantity_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            0,
            25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFolioItem_WithNegativeAmount_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            1,
            -25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFolioItem_WithEmptyTransactionCodeId_ReturnsBadRequest()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            1,
            25m,
            Guid.Empty);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}