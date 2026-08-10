using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.FiscalAccounting.Commands;

[CheckPermission(Permission.FiscalAccountEdit)]
public record OpenFolioCommand(Guid FiscalAccountId) : ICommand<Guid>;
