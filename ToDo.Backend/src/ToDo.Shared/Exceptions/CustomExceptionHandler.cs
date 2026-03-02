using System.Net;

using FluentValidation;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ToDo.Shared.Exceptions;

namespace ToDO.Shared.Exceptions;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception e, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exMessage}, time of occurrence {exTime}",
            e.Message, DateTime.UtcNow);

        (HttpStatusCode statusCode, string errorMessage) = e switch
        {
            OperationCanceledException operationCanceledEx => (HttpStatusCode.BadRequest, operationCanceledEx.Message),
            //BadRequestException badRequestEx => (HttpStatusCode.BadRequest, badRequestEx.Message),
            //InternalServerException internalServerEx => (HttpStatusCode.InternalServerError, internalServerEx.Message),
            NotFoundException notFoundEx => (HttpStatusCode.NotFound, notFoundEx.Message),
            ValidationException validationEx => (HttpStatusCode.BadRequest, validationEx.Message),
            UnauthorizedAccessException unauthorizedEx => (HttpStatusCode.Unauthorized, unauthorizedEx.Message),
            _ => (HttpStatusCode.InternalServerError, e.Message)
        };

        ProblemDetails problemDetails = new()
        {
            Detail = errorMessage,
            Type = e.GetType().Name,
            Status = (int)statusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if (e is ValidationException ve)
        {
            problemDetails.Extensions.Add("validationErrors", ve.Errors);
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}