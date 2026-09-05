using Hotel.Domain.Transactions.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionCodeEdit)]
public record UpdateTransactionCodeCommand(
    Guid Id,
    Guid TransactionGroupId,
    string Code,
    string Name)
    : ICommand;

public class UpdateTransactionCodeCommandHandler(ITransactionCodeUpdateService transactionCodeUpdateService)
    : IRequestHandler<UpdateTransactionCodeCommand>
{
    public async Task Handle(UpdateTransactionCodeCommand request, CancellationToken cancellationToken)
    {
        await transactionCodeUpdateService.UpdateTransactionCode(
            request.Id,
            request.TransactionGroupId,
            request.Code,
            request.Name,
            cancellationToken);
    }
}
