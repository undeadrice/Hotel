using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionCodeView)]
public record GetTransactionCodesSimpleListQuery()
    : IRequest<IReadOnlyCollection<TransactionCodeSimpleListDto>>;

internal class GetTransactionCodesSimpleListQueryHandler(
    ITransactionCodeReadRepository transactionCodeReadRepository)
    : IRequestHandler<GetTransactionCodesSimpleListQuery, IReadOnlyCollection<TransactionCodeSimpleListDto>>
{
    public async Task<IReadOnlyCollection<TransactionCodeSimpleListDto>> Handle(
        GetTransactionCodesSimpleListQuery request,
        CancellationToken cancellationToken)
    {
        return await transactionCodeReadRepository.GetActiveSimpleList(cancellationToken);
    }
}
