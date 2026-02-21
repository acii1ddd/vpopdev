using MediatR;

namespace ToDO.Shared.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}

// void result
public interface ICommand : ICommand<Unit>
{
}