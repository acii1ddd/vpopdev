using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;
using ToDO.Shared.CQRS;

namespace ToDo.API.Features.ToDos.GetTodos;


public sealed record GetTodosQuery : IQuery<GetTodosResult>;

public sealed record GetTodosResult(IEnumerable<ToDoItem> Todos);

public sealed record GetTodosQueryHandler(IToDoItemRepository ToDoItemRepository)
    : IQueryHandler<GetTodosQuery, GetTodosResult>
{
    public async Task<GetTodosResult> Handle(GetTodosQuery query, CancellationToken ct)
    {
        var todos = await ToDoItemRepository.GetAllAsync(ct);
        
        return new GetTodosResult(todos);
    }
}

