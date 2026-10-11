using FluentValidation;
using Hotel.Application.Rooming.Commands;

namespace Hotel.Application.Rooming.Validators;

public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand>
{
    public UpdateRoomTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room type id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Room type name is required.")
            .MaximumLength(100).WithMessage("Room type name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Room type description must not exceed 500 characters.");
    }
}