using System.Net;

namespace HE.Investments.Common.CRM.Exceptions;

public sealed class CrmApiCommunicationException : CrmException
{
    public CrmApiCommunicationException(HttpStatusCode statusCode, Exception? innerException = null, string? errorContent = null)
        : base($"Communication error occurred while accessing CRM API, HTTP code: {statusCode}.", innerException)
    {
        StatusCode = statusCode;
        ErrorContent = errorContent;
    }

    public HttpStatusCode StatusCode { get; }

    public string? ErrorContent { get; }
}
