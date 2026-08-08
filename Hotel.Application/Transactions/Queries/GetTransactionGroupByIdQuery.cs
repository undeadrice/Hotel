using Hotel.Application.Transactions.TransferObjects;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

public record GetTransactionGroupByIdQuery(Guid Id) : IRequest<TransactionGroupDto>;
