using FluentAssertions;
using Hotel.Application.NumberCycles.TransferObjects;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.IntegrationTests.Infrastructure;
using Hotel.IntegrationTests.Infrastructure.TestData;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Hotel.IntegrationTests.NumberCycles.Queries;

public class GetNumberCyclesQueryTests : IClassFixture<HotelWebApplicationFactory>, IAsyncLifetime
{
    private readonly HotelWebApplicationFactory _factory;

    private HttpClient _client = null!;

    public GetNumberCyclesQueryTests(HotelWebApplicationFactory factory)
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
    public async Task GetNumberCycles_WhenCyclesExist_ReturnsCycles()
    {
        // Arrange
        var reservationCycleId = await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.Reservation, "RES", 1);
        var fiscalAccountCycleId = await NumberCycleTestData.CreateNumberCycleAsync(_client, NumberCycleTopic.FiscalAccount, "FA", 5);

        // Act
        var response = await _client.GetAsync("/api/numbercycles");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var numberCycles = await response.Content.ReadFromJsonAsync<List<NumberCycleDto>>();
        numberCycles.Should().NotBeNull();
        numberCycles!.Should().HaveCount(2);

        numberCycles.Should().ContainSingle(c =>
            c.Id == reservationCycleId &&
            c.Topic == NumberCycleTopic.Reservation &&
            c.Prefix == "RES" &&
            c.StartIndex == 1 &&
            c.CurrentIndex == 1);

        numberCycles.Should().ContainSingle(c =>
            c.Id == fiscalAccountCycleId &&
            c.Topic == NumberCycleTopic.FiscalAccount &&
            c.Prefix == "FA" &&
            c.StartIndex == 5 &&
            c.CurrentIndex == 5);
    }
}