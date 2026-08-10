using Hotel.Application.Pipeline;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionGroupView)]
public record GetTransactionGroupsQuery(bool? IsActive = null)
    : IRequest<IReadOnlyCollection<TransactionGroupListDto>>;
