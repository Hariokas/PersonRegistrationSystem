using System.Net;
using Newtonsoft.Json;
using Serilog;
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
        context.Response.StatusCode = exception switch
        {
            UserNotFoundException => (int)HttpStatusCode.NotFound,
            UserAlreadyExistsException => (int)HttpStatusCode.Conflict,
            InvalidCredentialsException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Forbidden,
            PersonNotFoundException => (int)HttpStatusCode.NotFound,
            ResidenceNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var correlationId = context.TraceIdentifier; // or use your own correlation ID generator
        var result = JsonConvert.SerializeObject(new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
            CorrelationId = correlationId
        });

        Log.Error(exception.Message);

        return context.Response.WriteAsync(result);
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = "Unknown error";
        public string CorrelationId { get; set; } = "N/A";
    }
}