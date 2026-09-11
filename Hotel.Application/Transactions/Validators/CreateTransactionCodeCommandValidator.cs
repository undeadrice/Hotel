using FluentValidation;
using Hotel.Application.Transactions.Commands;

namespace Hotel.Application.Transactions.Validators;

public class CreateTransactionCodeCommandValidator : AbstractValidator<CreateTransactionCodeCommand>
{
    public CreateTransactionCodeCommandValidator()
    {
        RuleFor(x => x.TransactionGroupId)
            .NotEmpty().WithMessage("Transaction group is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(20).WithMessage("Code must not exceed 20 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    }
}
