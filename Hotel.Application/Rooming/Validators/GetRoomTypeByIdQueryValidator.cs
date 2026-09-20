using FluentValidation;
using Hotel.Application.Rooming.Queries;

namespace Hotel.Application.Rooming.Validators;

public class GetRoomTypeByIdQueryValidator : AbstractValidator<GetRoomTypeByIdQuery>
{
    public GetRoomTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room type id is required.");
    }
}