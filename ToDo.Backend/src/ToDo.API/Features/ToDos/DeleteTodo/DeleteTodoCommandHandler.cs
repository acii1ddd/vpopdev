using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDO.Shared.CQRS;
using ToDo.Shared.Exceptions;
using ToDO.Shared.Exceptions;

namespace ToDo.API.Features.ToDos.DeleteTodo;

public sealed record DeleteTodoCommand(Guid Id) : ICommand<DeleteTodoResult>;

public sealed record DeleteTodoResult(ToDoItem Item);

public sealed record DeleteTodoCommandHandler(IToDoItemRepository ToDoItemRepository)
    : ICommandHandler<DeleteTodoCommand, DeleteTodoResult>
{
    public async Task<DeleteTodoResult> Handle(DeleteTodoCommand command, CancellationToken ct)
    {
        var todoResult = await ToDoItemRepository.GetByIdAsync(command.Id, ct);

        if (todoResult == null)
        {
            throw new NotFoundException($"Cannot find entity with this id: {command.Id}");
        }
        
        await ToDoItemRepository.DeleteAsync(todoResult, ct);
        
        return new DeleteTodoResult(todoResult);
    }
}