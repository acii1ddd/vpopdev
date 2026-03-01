using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDo.API.Data.Models.Enums;
using ToDO.Shared.CQRS;

namespace ToDo.API.Features.ToDos.AddTodo;

public sealed record AddTodoCommand(
    string Title,
    string Description,
    Priority Priority
) : ICommand<AddTodoResult>;

public sealed record AddTodoResult(ToDoItem Item); // todo make AddToDoResponse with real user model

public sealed record AddTodoCommandHandler(IToDoItemRepository ToDoItemRepository)
    : ICommandHandler<AddTodoCommand, AddTodoResult>
{
    // TODO: replace with real user id from auth
    private static readonly Guid StaticUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public async Task<AddTodoResult> Handle(AddTodoCommand command, CancellationToken ct)
    {
        var todo = new ToDoItem
        {
            Id = Guid.NewGuid(),
            UserId = StaticUserId,
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
