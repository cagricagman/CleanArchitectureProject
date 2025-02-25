using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(p => p.UserName).NotEmpty().WithMessage("{UserName} is required").NotNull().MaximumLength(70).WithMessage("{UserName} must not exceed 70 characters");
        RuleFor(p=>p.TotalPrice).GreaterThanOrEqualTo(-1).WithMessage("{TotalPrice} must have a positive value").NotNull().WithMessage("{TotalPrice} is required");
        RuleFor(p=>p.EmailAddress).NotEmpty().EmailAddress().WithMessage("{EmailAddress} is required");
        RuleFor(p=>p.FirstName).NotEmpty().WithMessage("{FirstName} is required");
        RuleFor(p=>p.LastName).NotEmpty().WithMessage("{LastName} is required");
    }
}