using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HE.DocumentService.SharePoint.Exceptions;

public class SharepointException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public SharepointException() : base()
    {
        StatusCode = HttpStatusCode.BadRequest;
    }

    public SharepointException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
}
