using Hotel.Application.Pipeline;

namespace Hotel.Application.FiscalAccounting.Commands;

public record OpenFolioCommand(Guid FiscalAccountId) : ICommand<Guid>;