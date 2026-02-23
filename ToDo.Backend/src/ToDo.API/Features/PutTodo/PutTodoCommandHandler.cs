using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDo.API.Data.Models.Enums;
using ToDO.Shared.CQRS;
using ToDO.Shared.Exceptions;

namespace ToDo.API.Features.PutTodo;

public sealed record PutTodoCommand(
    Guid Id,
    string? Title = null,
    string? Description = null,
    bool? IsCompleted = null,
    Priority? Priority = null
) : ICommand<PutTodoResult>;

public sealed record PutTodoResult(ToDoItem Item);

public sealed record PutTodoCommandHandler(IToDoItemRepository ToDoItemRepository) 
    : ICommandHandler<PutTodoCommand, PutTodoResult>
{
    public async Task<PutTodoResult> Handle(PutTodoCommand command, CancellationToken ct)
    {
        var todo = await ToDoItemRepository.GetByIdAsync(command.Id, ct);

        if (todo == null)
        {
            throw new NotFoundException($"Cannot find entity with this id: {command.Id}");
        }
        
        if (command.Title is not null)
            todo.Title = command.Title;
        
        if (command.Description is not null)
            todo.Description = command.Description;
        
        if (command.IsCompleted.HasValue)
            todo.IsCompleted = command.IsCompleted.Value;
        
        if (command.Priority.HasValue)
            todo.Priority = command.Priority.Value;

        todo.UpdatedAt = DateTime.UtcNow;

        await ToDoItemRepository.UpdateAsync(todo, ct);

        return new PutTodoResult(todo);
    }
}