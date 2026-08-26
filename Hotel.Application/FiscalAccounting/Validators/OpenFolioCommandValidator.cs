using FluentValidation;
using Hotel.Application.FiscalAccounting.Commands;

namespace Hotel.Application.FiscalAccounting.Validators;

public class OpenFolioCommandValidator : AbstractValidator<OpenFolioCommand>
{
    public OpenFolioCommandValidator()
    {
        RuleFor(x => x.FiscalAccountId)
            .NotEmpty().WithMessage("Fiscal account is required.");
    }
}