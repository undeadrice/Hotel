using FluentValidation;
using Hotel.Application.Transactions.Commands;

namespace Hotel.Application.Transactions.Validators;

public class UpdateTransactionGroupCommandValidator : AbstractValidator<UpdateTransactionGroupCommand>
{
    public UpdateTransactionGroupCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(20).WithMessage("Code must not exceed 20 characters.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("A valid transaction type is required.");
    }
}
