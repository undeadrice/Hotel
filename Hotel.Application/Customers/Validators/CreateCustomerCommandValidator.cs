using FluentValidation;
using Hotel.Application.Customers.Commands;
using Hotel.Domain.Customers;

namespace Hotel.Application.Customers.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Location)
            .IsInEnum().WithMessage($"Location must be one of: {string.Join(", ", Enum.GetNames<CustomerLocation>())}.");
    }
}
