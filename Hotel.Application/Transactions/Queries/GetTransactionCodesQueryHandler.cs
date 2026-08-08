using Hotel.Application.Transactions.Services;
using Hotel.Application.Transactions.TransferObjects;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

internal class GetTransactionCodesQueryHandler(ITransactionCodeReadRepository transactionCodeReadRepository)
    : IRequestHandler<GetTransactionCodesQuery, IReadOnlyCollection<TransactionCodeListDto>>
{
    public async Task<IReadOnlyCollection<TransactionCodeListDto>> Handle(
        GetTransactionCodesQuery request,
        CancellationToken cancellationToken)
    {
        return await transactionCodeReadRepository.GetAll(
            request.TransactionGroupId,
            request.IsActive,
            cancellationToken);
    }
}
