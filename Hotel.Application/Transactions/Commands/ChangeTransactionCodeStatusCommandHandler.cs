using MediatR;
using Hotel.Domain.Transactions.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Commands;

[CheckPermission(Permission.TransactionCodeEdit)]
public record ChangeTransactionCodeStatusCommand(
    Guid Id,
    bool IsActive)
    : ICommand;

public class ChangeTransactionCodeStatusCommandHandler(ITransactionCodeRepository transactionCodeRepository)
    : IRequestHandler<ChangeTransactionCodeStatusCommand>
{
    public async Task Handle(ChangeTransactionCodeStatusCommand request, CancellationToken cancellationToken)
    {
        var transactionCode = await transactionCodeRepository.GetById(request.Id, cancellationToken);

        if (request.IsActive)
        {
            transactionCode.Activate();
        }
        else
        {
            transactionCode.Deactivate();
        }
    }
}
