using FluentValidation;

using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;

using ToDO.Shared.CQRS;

using ToDo.Shared.Exceptions;

namespace ToDo.API.Features.ToDos.GetTodo;

public sealed record GetTodoQuery(Guid Id) : IQuery<GetTodoResult>;

public class GetTodoQueryValidator : AbstractValidator<GetTodoQuery>
{
    public GetTodoQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("Id cannot be empty");
    }
}

public sealed record GetTodoResult(ToDoItem Item); // todo make AddToDoResponse with real user modelx   

public sealed record GetTodoQueryHandler(IToDoItemRepository ToDoItemRepository)
    : IQueryHandler<GetTodoQuery, GetTodoResult>
{
    public async Task<GetTodoResult> Handle(GetTodoQuery query, CancellationToken ct)
    {
        ToDoItem? todoResult = await ToDoItemRepository.GetByIdAsync(query.Id, ct);

        if (todoResult == null)
        {
            throw new NotFoundException($"Cannot find entity with this id: {query.Id}");
        }

        return new GetTodoResult(todoResult);
    }
}