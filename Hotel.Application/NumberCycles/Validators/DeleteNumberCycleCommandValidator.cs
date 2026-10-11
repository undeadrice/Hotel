using FluentValidation;
using Hotel.Application.NumberCycles.Commands;

namespace Hotel.Application.NumberCycles.Validators;

public class DeleteNumberCycleCommandValidator : AbstractValidator<DeleteNumberCycleCommand>
{
    public DeleteNumberCycleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Number cycle id is required.");
    }
}