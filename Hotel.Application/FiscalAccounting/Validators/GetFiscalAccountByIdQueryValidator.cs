using FluentValidation;
using Hotel.Application.FiscalAccounting.Queries;

namespace Hotel.Application.FiscalAccounting.Validators;

public class GetFiscalAccountByIdQueryValidator : AbstractValidator<GetFiscalAccountByIdQuery>
{
    public GetFiscalAccountByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Fiscal account is required.");
    }
}