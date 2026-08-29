using Hotel.Domain.Transactions.Services;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using Hotel.Domain.Transactions.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionGroupCreate)]
public record CreateTransactionGroupCommand(
    string Code,
    string Name,
    TransactionType Type)
    : ICommand<Guid>;

public class CreateTransactionGroupCommandHandler(ITransactionGroupCreationService transactionGroupCreationService)
    : IRequestHandler<CreateTransactionGroupCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionGroupCommand request, CancellationToken cancellationToken)
    {
        var transactionGroup = await transactionGroupCreationService.CreateTransactionGroup(
            request.Code,
            request.Name,
            request.Type,
            cancellationToken);

        return transactionGroup.Id;
    }
}
