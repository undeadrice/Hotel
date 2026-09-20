using FluentValidation;
using Hotel.Application.FiscalAccounting.Commands;

namespace Hotel.Application.FiscalAccounting.Validators;

public class CheckOutFiscalAccountCommandValidator : AbstractValidator<CheckOutFiscalAccountCommand>
{
    public CheckOutFiscalAccountCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account is required.");
    }
}