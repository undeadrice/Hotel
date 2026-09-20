using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionCodeView)]
public record GetTransactionCodeByIdQuery(Guid Id) : IRequest<TransactionCodeDto>;

internal class GetTransactionCodeByIdQueryHandler(ITransactionCodeReadRepository transactionCodeReadRepository)
    : IRequestHandler<GetTransactionCodeByIdQuery, TransactionCodeDto>
{
    public async Task<TransactionCodeDto> Handle(
        GetTransactionCodeByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await transactionCodeReadRepository.GetById(request.Id, cancellationToken);
    }
}
