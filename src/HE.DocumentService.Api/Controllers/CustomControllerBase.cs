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

    public T Service<T>() => (T)(_contextAccessor.HttpContext?.RequestServices.GetService(typeof(T)) ?? throw new ArgumentNullException());
}
