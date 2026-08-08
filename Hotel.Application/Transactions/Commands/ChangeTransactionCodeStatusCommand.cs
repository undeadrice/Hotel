using Hotel.Application.Pipeline;

namespace Hotel.Application.Transactions.Commands;

public record ChangeTransactionCodeStatusCommand(
    Guid Id,
    bool IsActive)
    : ICommand;
