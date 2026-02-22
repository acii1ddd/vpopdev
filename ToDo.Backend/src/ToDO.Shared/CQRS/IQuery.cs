using MediatR;

namespace ToDO.Shared.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}