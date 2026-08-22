using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;

namespace Hotel.Application.Transactions.Queries;

internal class GetTransactionGroupByIdQueryHandler(ITransactionGroupReadRepository transactionGroupReadRepository)
    : IRequestHandler<GetTransactionGroupByIdQuery, TransactionGroupDto>
{
    public async Task<TransactionGroupDto> Handle(
        GetTransactionGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await transactionGroupReadRepository.GetById(request.Id, cancellationToken);
    }
}
