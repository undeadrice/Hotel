using Hotel.Application.Transactions.TransferObjects;
using MediatR;
using Hotel.Application.Transactions.Repositories;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionGroupView)]
public record GetTransactionGroupsQuery(bool? IsActive = null)
    : IRequest<IReadOnlyCollection<TransactionGroupListDto>>;

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
