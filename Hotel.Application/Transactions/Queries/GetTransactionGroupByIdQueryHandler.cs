using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionGroupView)]
public record GetTransactionGroupByIdQuery(Guid Id) : IRequest<TransactionGroupDto>;

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
