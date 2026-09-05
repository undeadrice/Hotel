using Hotel.Domain.Transactions.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionGroupEdit)]
public record UpdateTransactionGroupCommand(
    Guid Id,
    string Code,
    string Name,
    TransactionType Type)
    : ICommand;

public class UpdateTransactionGroupCommandHandler(ITransactionGroupUpdateService transactionGroupUpdateService)
    : IRequestHandler<UpdateTransactionGroupCommand>
{
    public async Task Handle(UpdateTransactionGroupCommand request, CancellationToken cancellationToken)
    {
        await transactionGroupUpdateService.UpdateTransactionGroup(
            request.Id,
            request.Code,
            request.Name,
            request.Type,
            cancellationToken);
    }
}
