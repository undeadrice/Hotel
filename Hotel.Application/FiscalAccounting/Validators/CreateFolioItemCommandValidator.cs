using FluentValidation;
using Hotel.Application.FiscalAccounting.Commands;

namespace Hotel.Application.FiscalAccounting.Validators;

public class CreateFolioItemCommandValidator : AbstractValidator<CreateFolioItemCommand>
{
    public CreateFolioItemCommandValidator()
    {
        RuleFor(x => x.FolioId)
            .NotEmpty().WithMessage("Folio is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be zero or greater.");

        RuleFor(x => x.TransactionCodeId)
            .NotEmpty().WithMessage("Transaction code is required.");
    }
}