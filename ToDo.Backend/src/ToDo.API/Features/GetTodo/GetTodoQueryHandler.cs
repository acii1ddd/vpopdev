using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDO.Shared.CQRS;
using ToDO.Shared.Exceptions;

namespace ToDo.API.Features.GetTodo;

public sealed record GetTodoQuery(Guid Id) : IQuery<GetTodoResult>;

public sealed record GetTodoResult(ToDoItem Item);

public sealed record GetTodoQueryHandler(IToDoItemRepository ToDoItemRepository) 
    : IQueryHandler<GetTodoQuery, GetTodoResult>
{
    public async Task<GetTodoResult> Handle(GetTodoQuery query, CancellationToken ct)
    {
        var todoResult = await ToDoItemRepository.GetByIdAsync(query.Id, ct);
        
        if (todoResult == null)
        {
            throw new NotFoundException($"Cannot find entity with this id: {query.Id}");
        }
        
        return new GetTodoResult(todoResult);
    }
}