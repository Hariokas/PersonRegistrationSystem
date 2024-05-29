using System.Net;
using Newtonsoft.Json;
using Shared;

namespace API.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionTask(context, ex);
        }
    }

    private Task HandleExceptionTask(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            UserNotFoundException => (int)HttpStatusCode.NotFound,
            UserAlreadyExistsException => (int)HttpStatusCode.Conflict,
            InvalidCredentialsException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var result = JsonConvert.SerializeObject(new { error = exception.Message });

        return context.Response.WriteAsync(result);
    }
}