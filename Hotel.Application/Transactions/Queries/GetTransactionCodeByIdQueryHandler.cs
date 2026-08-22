using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;

namespace Hotel.Application.Transactions.Queries;

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
