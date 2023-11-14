using System.Net;

namespace HE.Investments.DocumentService.Exceptions;

public class DocumentServiceCommunicationException : DocumentServiceException
{
    public DocumentServiceCommunicationException(
        HttpMethod httpMethod,
        Uri? requestUrl,
        HttpStatusCode documentServiceStatusCode,
        string documentServiceResponse)
        : base("There was a problem with the Document Service connection.")
    {
        HttpMethod = httpMethod;
        RequestUrl = requestUrl;
        DocumentServiceStatusCode = documentServiceStatusCode;
        DocumentServiceResponse = documentServiceResponse;
    }

    public HttpMethod HttpMethod { get; }

    public Uri? RequestUrl { get; }

    public HttpStatusCode DocumentServiceStatusCode { get; }

    public string DocumentServiceResponse { get; }
}
