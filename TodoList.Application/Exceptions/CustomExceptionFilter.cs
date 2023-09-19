using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TodoList.Application.Exceptions;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Validation failed.";
                break;
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred. Please try again later.";
                break;
        }

        context.Result = new ObjectResult(new { error = message })
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}
