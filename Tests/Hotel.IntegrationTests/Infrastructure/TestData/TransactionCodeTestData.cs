using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class TransactionCodeTestData
{
    public static async Task<Guid> CreateTransactionCodeAsync(
        HttpClient client,
        Guid transactionGroupId,
        string code = "1001",
        string name = "Default transaction code")
    {
        var response = await client.PostAsJsonAsync(
            "/api/transactioncodes",
            new CreateTransactionCodeCommand(transactionGroupId, code, name));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}