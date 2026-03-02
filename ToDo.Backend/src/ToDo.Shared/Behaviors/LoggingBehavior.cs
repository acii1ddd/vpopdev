using System.Diagnostics;

using MediatR;

using Microsoft.Extensions.Logging;

namespace ToDo.Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        logger.LogInformation("[START] Handling request {RequestName} ({@Request})",
            typeof(TRequest).Name, request);

        // other behaviors -> handler -> other behaviors -> response
        Stopwatch stopwatch = Stopwatch.StartNew();
        TResponse response = await next(ct);
        stopwatch.Stop();

        logger.LogInformation(
            "[END] Request {RequestName} handled - response: {@Response} it takes {ElapsedMilliseconds} ms",
            typeof(TRequest).Name, response, stopwatch.ElapsedMilliseconds);

        return response;
    }
}