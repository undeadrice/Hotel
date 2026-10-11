using FluentValidation;
using Hotel.Application.NumberCycles.Commands;
using Hotel.Domain.NumberCycles.Enums;

namespace Hotel.Application.NumberCycles.Validators;

public class CreateNumberCycleCommandValidator : AbstractValidator<CreateNumberCycleCommand>
{
    public CreateNumberCycleCommandValidator()
    {
        RuleFor(x => x.Topic)
            .IsInEnum().WithMessage("Invalid number cycle topic.");

        RuleFor(x => x.Prefix)
            .NotEmpty().WithMessage("Prefix is required.")
            .MaximumLength(20).WithMessage("Prefix must not exceed 20 characters.");

        RuleFor(x => x.StartIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Start index must be zero or greater.");
    }
}