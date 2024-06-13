using System.Net;

namespace HE.Investments.Api.Exceptions;

public sealed class ApiCommunicationException : ApiException
{
    public ApiCommunicationException(HttpStatusCode statusCode, Exception? innerException = null, string? errorContent = null)
        : base($"Communication error occurred while accessing Investments API, HTTP code: {statusCode}.", innerException)
    {
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }

    public HttpStatusCode StatusCode { get; }

    public string? ErrorContent { get; }
}
