using BuildingBlocks.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoHub.API.Models;
using TodoHub.Domain.Exceptions;

namespace TodoHub.API.Middlewares;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {message}, Time of occurrence: {time}", exception.Message, DateTime.UtcNow);

        (string Details, string Title, int StatusCode) details = exception switch
        {
            ForbiddenException => 
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
            ValidationException => 
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
            BadRequestException => 
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
            BadHttpRequestException => 
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
            NotFoundException => 
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),
            _ => 
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError 
                )
        };
        
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Details,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path,
        };
        
        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
        
        var response = ApiResponse<object>.ErrorResult(details.Title, [details.Details]);
        if (exception is ValidationException validationException)
        {
            var validationErrors = validationException.Errors
                .Select(x => x.ErrorMessage)
                .ToList();
            response.Errors.AddRange(validationErrors);
        }
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        return true;
    }
}