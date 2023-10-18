using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HE.Investments.DocumentService.Exceptions;

public class DocumentServiceException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public DocumentServiceException() : base()
    {
        StatusCode = HttpStatusCode.BadRequest;
    }

    public DocumentServiceException(string message) : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
}
