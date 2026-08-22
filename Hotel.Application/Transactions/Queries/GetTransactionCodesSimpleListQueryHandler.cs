using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;

namespace Hotel.Application.Transactions.Queries;

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