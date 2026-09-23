using Hotel.Domain.Transactions.Services;
using MediatR;
using Hotel.SharedPipeline;
using Hotel.SharedPipeline.Attributes;
using Hotel.Shared.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionCodeCreate)]
public record CreateTransactionCodeCommand(
    Guid TransactionGroupId,
    string Code,
    string Name)
    : ICommand<Guid>;

internal class CreateTransactionCodeCommandHandler(ITransactionCodeCreationService transactionCodeCreationService)
    : IRequestHandler<CreateTransactionCodeCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransactionCodeCommand request, CancellationToken cancellationToken)
    {
        var transactionCode = await transactionCodeCreationService.CreateTransactionCode(
            request.TransactionGroupId,
            request.Code,
            request.Name,
            cancellationToken);

        return transactionCode.Id;
    }
}
