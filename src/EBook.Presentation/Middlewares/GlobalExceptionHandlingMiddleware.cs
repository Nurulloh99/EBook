using EBook.Core.Errors;
using EBook.Errors;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace EBook.Server.Middlewares;

public class GlobalExceptionHandlingMiddleware(RequestDelegate _next, ILogger<GlobalExceptionHandlingMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, DateTime.Now);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var message = exception.Message;

        var code = 500;
        context.Response.ContentType = "application/json";

        if (exception is EntityNotFoundException || exception is ArgumentNullException)
        {
            code = 404;
            message = "Not found";
        }
        else if (exception is AuthException || exception is UnauthorizedException)
        {
            code = 401;
            message = "This resource is not available to unauthenticated users. Please try to logIn again!";
        }
        else if (exception is ForbiddenException || exception is NotAllowedException)
        {
            code = 403;
            message = "Not allowed";
        }
        else if (exception is BadRequest)
        {
            code = 400;
            message = "Incorrect request";
        }
        else if (exception is UnprocessableEntity)
        {
            code = 422;
            message = "Validation error";
        }

        context.Response.StatusCode = code;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            Detail = exception.Message
        };

        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }
}
