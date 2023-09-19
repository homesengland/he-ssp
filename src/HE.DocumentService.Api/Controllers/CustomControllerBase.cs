using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc;

namespace HE.DocumentService.Api.Controllers;

public class CustomControllerBase : ControllerBase
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CustomControllerBase(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public T Service<T>() => (T)_contextAccessor.HttpContext.RequestServices.GetService(typeof(T));

    public override FileContentResult File(byte[] fileContents, string contentType, string fileDownloadName)
    {
        Response.Headers.Add("File-Name", fileDownloadName);
        Response.Headers.Add("Access-Control-Expose-Headers", "File-Name");

        return base.File(fileContents, contentType, fileDownloadName);
    }
}
