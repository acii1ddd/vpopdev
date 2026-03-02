using FluentValidation;

using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDo.API.Data.Models.Enums;

using ToDO.Shared.CQRS;

namespace ToDo.API.Features.ToDos.AddTodo;

public sealed record AddTodoCommand(
    string Title,
    string Description,
    Priority Priority,
    Guid UserId
) : ICommand<AddTodoResult>;

public sealed class AddTodoCommandValidator : AbstractValidator<AddTodoCommand>
{
    public AddTodoCommandValidator()
    {
        // Id: required, valid guid
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId cannot be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("UserId cannot be empty");

        // Title: required, 3-256 characters
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MinimumLength(3)
            .WithMessage("Title must be at least 3 characters long")
            .MaximumLength(256)
            .WithMessage("Title cannot exceed 256 characters")
            .Matches(@"^[a-zA-Zа-яА-Я0-9\s\-_!?.]+$")
            .WithMessage("Title contains invalid characters");

        // Description: required, 5-256 characters
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MinimumLength(5)
            .WithMessage("Description must be at least 5 characters long")
            .MaximumLength(256)
            .WithMessage("Description cannot exceed 256 characters");

        // Priority: must be a valid enum value
        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid priority value")
            .Must(BeValidPriority)
            .WithMessage("Priority must be Low, Medium, or High");
    }

    // Additional validation for enum
    private static bool BeValidPriority(Priority priority)
    {
        return priority is Priority.Low or Priority.Medium or Priority.High;
    }
}

public sealed record AddTodoResult(ToDoItem Item); // todo make AddToDoResponse with real user model

public sealed record AddTodoCommandHandler(
    IToDoItemRepository ToDoItemRepository)
    : ICommandHandler<AddTodoCommand, AddTodoResult>
{
    public async Task<AddTodoResult> Handle(AddTodoCommand command, CancellationToken ct)
    {
        ToDoItem todo = new()
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Title = command.Title,
            Description = command.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Priority = command.Priority
        };

        await ToDoItemRepository.AddAsync(todo, ct);

        return new AddTodoResult(todo);
    }
}