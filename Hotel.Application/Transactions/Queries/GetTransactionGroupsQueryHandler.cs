using Hotel.Application.Transactions.Services;
using Hotel.Application.Transactions.TransferObjects;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

internal class GetTransactionGroupsQueryHandler(ITransactionGroupReadRepository transactionGroupReadRepository)
    : IRequestHandler<GetTransactionGroupsQuery, IReadOnlyCollection<TransactionGroupListDto>>
{
    public async Task<IReadOnlyCollection<TransactionGroupListDto>> Handle(
        GetTransactionGroupsQuery request,
        CancellationToken cancellationToken)
    {
        return await transactionGroupReadRepository.GetAll(request.IsActive, cancellationToken);
    }
}
