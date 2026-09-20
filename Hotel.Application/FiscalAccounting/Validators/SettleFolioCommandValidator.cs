using FluentValidation;
using Hotel.Application.FiscalAccounting.Commands;

namespace Hotel.Application.FiscalAccounting.Validators;

public class SettleFolioCommandValidator : AbstractValidator<SettleFolioCommand>
{
    public SettleFolioCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account is required.");

        RuleFor(x => x.FolioId)
            .NotEmpty().WithMessage("Folio is required.");
    }
}