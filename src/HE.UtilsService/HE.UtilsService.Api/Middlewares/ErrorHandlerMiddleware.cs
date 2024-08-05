using System.Net;
using System.Text.Json;
using HE.DocumentService.SharePoint.Exceptions;

namespace HE.DocumentService.Api.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string? result;
            switch (error)
            {
                case SharepointException e:
                    // custom sharepoint error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { message = error.Message, statusCode = e.StatusCode.ToString() });
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result = JsonSerializer.Serialize(new { message = "Internal server error. Please contact your administrator for details." });
                    break;
            }

            await response.WriteAsync(result);
        }
    }
}
