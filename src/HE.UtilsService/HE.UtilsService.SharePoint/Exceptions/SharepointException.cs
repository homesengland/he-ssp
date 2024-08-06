using System.Net;

namespace HE.UtilsService.SharePoint.Exceptions;

public class SharepointException : Exception
{
    public SharepointException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }

    public HttpStatusCode StatusCode { get; }
}
