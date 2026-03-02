using FluentValidation;

using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDo.API.Data.Models.Enums;

using ToDO.Shared.CQRS;

using ToDo.Shared.Exceptions;

namespace ToDo.API.Features.ToDos.PutTodo;

public sealed record PutTodoCommand(
    Guid Id,
    string? Title = null,
    string? Description = null,
    bool? IsCompleted = null,
    Priority? Priority = null
) : ICommand<PutTodoResult>;

public class PutTodoCommandValidator : AbstractValidator<PutTodoCommand>
{
    public PutTodoCommandValidator()
    {
        // Id: required, valid guid
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("Id cannot be empty");

        // Title: validate only not null values (for partial update)
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty")
            .MinimumLength(3)
            .WithMessage("Title must be at least 3 characters long")
            .MaximumLength(256)
            .WithMessage("Title cannot exceed 256 characters")
            .Matches(@"^[a-zA-Zа-яА-Я0-9\s\-_!?.]+$")
            .WithMessage("Title contains invalid characters")
            .When(x => x.Title is not null);

        // Description: validate only not null values (for partial update)
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty")
            .MinimumLength(5)
            .WithMessage("Description must be at least 5 characters long")
            .MaximumLength(256)
            .WithMessage("Description cannot exceed 256 characters")
            .When(x => x.Description is not null);

        // Priority: validate only not null values (for partial update)
        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid priority value")
            .Must(BeValidPriority)
            .WithMessage("Priority must be Low, Medium, or High")
            .When(x => x.Priority is not null);

        // IsCompleted: validation is not required (bool? is always valid)
    }

    // Additional validation for enum
    private static bool BeValidPriority(Priority? priority)
    {
        return priority is Priority.Low or Priority.Medium or Priority.High;
    }
}

public sealed record PutTodoResult(ToDoItem Item); // todo make AddToDoResponse with real user model

public sealed record PutTodoCommandHandler(IToDoItemRepository ToDoItemRepository)
    : ICommandHandler<PutTodoCommand, PutTodoResult>
{
    public async Task<PutTodoResult> Handle(PutTodoCommand command, CancellationToken ct)
    {
        ToDoItem? todo = await ToDoItemRepository.GetByIdAsync(command.Id, ct);

        if (todo == null)
        {
            throw new NotFoundException($"Cannot find entity with this id: {command.Id}");
        }

        if (command.Title is not null)
        {
            todo.Title = command.Title;
        }

        if (command.Description is not null)
        {
            todo.Description = command.Description;
        }

        if (command.IsCompleted.HasValue)
        {
            todo.IsCompleted = command.IsCompleted.Value;
        }

        if (command.Priority.HasValue)
        {
            todo.Priority = command.Priority.Value;
        }

        todo.UpdatedAt = DateTime.UtcNow;

        await ToDoItemRepository.UpdateAsync(todo, ct);

        return new PutTodoResult(todo);
    }
}