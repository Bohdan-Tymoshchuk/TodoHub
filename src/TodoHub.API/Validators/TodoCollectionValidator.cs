using FluentValidation;
using TodoHub.Application.DTOs;

namespace TodoHub.API.Validators;

public class TodoCollectionValidator : AbstractValidator<TodoCollectionDto>
{
    public TodoCollectionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Task list name is required")
            .Length(1, 255)
            .WithMessage("Task list name must be between 1 and 255 characters")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Task list name cannot contain only whitespace");
    }
}