using FluentValidation;

using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;

using ToDO.Shared.CQRS;

using ToDo.Shared.Exceptions;

namespace ToDo.API.Features.ToDos.DeleteTodo;

public sealed record DeleteTodoCommand(Guid Id) : ICommand<DeleteTodoResult>;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be empty")
            .NotEqual(Guid.Empty)
            .WithMessage("Id cannot be empty");
    }
}

public sealed record DeleteTodoResult(ToDoItem Item); // todo make AddToDoResponse with real user model

public sealed record DeleteTodoCommandHandler(IToDoItemRepository ToDoItemRepository)
    : ICommandHandler<DeleteTodoCommand, DeleteTodoResult>
{
    public async Task<DeleteTodoResult> Handle(DeleteTodoCommand command, CancellationToken ct)
    {
        ToDoItem? todoResult = await ToDoItemRepository.GetByIdAsync(command.Id, ct);

        if (todoResult == null)
        {
            throw new NotFoundException($"Cannot find entity with this id: {command.Id}");
        }

        await ToDoItemRepository.DeleteAsync(todoResult, ct);

        return new DeleteTodoResult(todoResult);
    }
}