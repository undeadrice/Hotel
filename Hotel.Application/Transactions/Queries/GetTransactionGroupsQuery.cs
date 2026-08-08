using Hotel.Application.Transactions.TransferObjects;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

public record GetTransactionGroupsQuery(bool? IsActive = null)
    : IRequest<IReadOnlyCollection<TransactionGroupListDto>>;
