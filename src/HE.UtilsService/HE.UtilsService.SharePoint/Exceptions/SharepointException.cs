using System.Net;

namespace HE.DocumentService.SharePoint.Exceptions;

public class SharepointException : Exception
{
    public SharepointException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }

    public HttpStatusCode StatusCode { get; }
}
