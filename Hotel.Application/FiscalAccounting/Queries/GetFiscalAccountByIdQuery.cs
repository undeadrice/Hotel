using Hotel.Application.FiscalAccounting.TransferObjects;
using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;
using MediatR;

namespace Hotel.Application.FiscalAccounting.Queries;

[CheckPermission(Permission.FiscalAccountView)]
public record GetFiscalAccountByIdQuery(Guid Id) : IRequest<FiscalAccountDetailsDto>;
