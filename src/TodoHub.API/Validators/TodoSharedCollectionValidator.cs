using FluentValidation;
using TodoHub.Application.DTOs;

namespace TodoHub.API.Validators;

public class TodoSharedCollectionValidator : AbstractValidator<TodoSharedCollectionDto>
{
    public TodoSharedCollectionValidator()
    {
        RuleFor(x => x.TodoCollectionId)
            .NotEmpty()
            .WithMessage("Collection ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}