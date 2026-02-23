using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ToDO.Shared.Exceptions;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception e, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exMessage}, time of occurrence {exTime}",
            e.Message, DateTime.UtcNow);
        
        var (statusCode, errorMessage) = e switch
        {
            OperationCanceledException operationCanceledEx => (HttpStatusCode.BadRequest, operationCanceledEx.Message),
            //BadRequestException badRequestEx => (HttpStatusCode.BadRequest, badRequestEx.Message),
            //InternalServerException internalServerEx => (HttpStatusCode.InternalServerError, internalServerEx.Message),
            NotFoundException notFoundEx => (HttpStatusCode.NotFound, notFoundEx.Message),
            ValidationException validationEx => (HttpStatusCode.BadRequest, validationEx.Message),
            _ => (HttpStatusCode.InternalServerError, e.Message)
        };
            
        var problemDetails = new ProblemDetails
        {
            Detail = errorMessage,
            Type = e.GetType().Name,
            Status = (int) statusCode,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);
        
        if (e is ValidationException ve)
        {
            problemDetails.Extensions.Add("validationErrors", ve.Errors);
        }
                
        context.Response.StatusCode = (int) statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}

public class NotFoundException(string message) : Exception(message);