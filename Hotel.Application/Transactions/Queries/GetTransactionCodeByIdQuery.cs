using Hotel.Application.Transactions.TransferObjects;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

public record GetTransactionCodeByIdQuery(Guid Id) : IRequest<TransactionCodeDto>;
