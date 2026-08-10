using Hotel.Application.Pipeline;
using Hotel.Application.Transactions.TransferObjects;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.Transactions.Queries;

[CheckPermission(Permission.TransactionCodeView)]
public record GetTransactionCodeByIdQuery(Guid Id) : IRequest<TransactionCodeDto>;
