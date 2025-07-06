using FluentValidation;
using TodoHub.Application.DTOs;

namespace TodoHub.API.Validators;

public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}