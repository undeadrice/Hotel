using FluentAssertions;
using Hotel.Application.Transactions.Commands;
using Hotel.Domain.Transactions.Enums;
using System.Net;
using System.Net.Http.Json;

namespace Hotel.IntegrationTests.Infrastructure.TestData;

public static class TransactionGroupTestData
{
    public static async Task<Guid> CreateTransactionGroupAsync(
        HttpClient client,
        string code = "1001",
        string name = "Charges",
        TransactionType type = TransactionType.Charge)
    {
        var response = await client.PostAsJsonAsync(
            "/api/transactiongroups",
            new CreateTransactionGroupCommand(code, name, type));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}