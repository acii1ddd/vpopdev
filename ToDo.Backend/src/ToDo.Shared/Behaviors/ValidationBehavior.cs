using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ToDo.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    ILogger<ValidationBehavior<TRequest, TResponse>> logger,
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken ct)
    {
        logger.LogInformation("Validating command {@Command}", request);
        
        var validationResults = await Task.WhenAll(
            validators.Select(v => 
                v.ValidateAsync(request, ct))
        );
        
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
            return await next(ct); // return response to logging behavior

        logger.LogWarning("Validation errors - {CommandType} - Command: " + "{@Command} - Errors: {@ValidationErrors}", 
            request.GetType(), request, failures);
            
        throw new ValidationException(failures);
    }
}