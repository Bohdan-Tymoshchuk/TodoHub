using FluentValidation;
using TodoHub.Application.DTOs;

namespace TodoHub.API.Validators;

public class TodoTaskValidator : AbstractValidator<TodoTaskDto>
{
    public TodoTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");
    }
}