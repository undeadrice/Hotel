using FluentAssertions;
using Hotel.Application.FiscalAccounting.Commands;
using Hotel.Domain.FiscalAccounting.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.FiscalAccounting.Commands;

public class CreateFolioItemCommandTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public CreateFolioItemCommandTests(HotelWebApplicationFactory factory)
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
    public async Task CreateFolioItem_WithChargeTransactionCode_ReturnsItemIdAndAddsCharge()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            Quantity: 2,
            Amount: 25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var itemId = await response.Content.ReadFromJsonAsync<Guid>();
        itemId.Should().NotBeEmpty();

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        var item = account.Folios.Single(f => f.Id == context.MainFolioId)
            .Items.Should().ContainSingle().Which;

        item.Id.Should().Be(itemId);
        item.TransactionType.Should().Be(FolioItemType.Charge);
        item.Amount.Should().Be(25m);
        item.Quantity.Should().Be(2);
    }

    [Fact]
    public async Task CreateFolioItem_WithPaymentTransactionCode_ReturnsItemIdAndAddsPayment()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);
        var paymentTransactionCodeId = await FiscalAccountTestData.CreatePaymentTransactionCodeAsync(_client);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Cash payment",
            Quantity: 1,
            Amount: 150m,
            paymentTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var itemId = await response.Content.ReadFromJsonAsync<Guid>();
        itemId.Should().NotBeEmpty();

        var account = await DataAccess.FiscalAccount(_factory)
            .Include(a => a.Folios)
            .ThenInclude(f => f.Items)
            .SingleAsync(a => a.Id == context.FiscalAccountId);

        var item = account.Folios.Single(f => f.Id == context.MainFolioId)
            .Items.Should().ContainSingle().Which;

        item.Id.Should().Be(itemId);
        item.TransactionType.Should().Be(FolioItemType.Payment);
        item.Amount.Should().Be(150m);
        item.Quantity.Should().Be(1);
    }

    [Fact]
    public async Task CreateFolioItem_WithNonExistentFolio_ReturnsNotFound()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            Guid.NewGuid(),
            "Room service",
            Quantity: 1,
            Amount: 25m,
            context.ChargeTransactionCodeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Extensions["exception"]!.ToString().Should().Be("NotFoundException");
    }

    [Fact]
    public async Task CreateFolioItem_WithNonExistentTransactionCode_ReturnsNotFound()
    {
        // Arrange
        var context = await FiscalAccountTestData.CreateContextAsync(_client, _factory);

        var command = new CreateFolioItemCommand(
            context.MainFolioId,
            "Room service",
            Quantity: 1,
            Amount: 25m,
            Guid.NewGuid());

        // Act
        var response = await _client.PostAsJsonAsync("/api/folioitems", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Extensions["exception"]!.ToString().Should().Be("NotFoundException");
    }
}
