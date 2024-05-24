using System.Text;
using HE.Investments.Account.Contract.Cache.Commands;
using HE.Investments.Account.Contract.Cache.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HE.Investments.Account.WWW.Controllers;

[Authorize]
[Route("api")]
public class ApiController : Controller
{
    private readonly IMediator _mediator;

    public ApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-user-cache")]
    public async Task<IActionResult> GetCache(CancellationToken cancellationToken)
    {
        var userCache = await _mediator.Send(new GetUserCacheQuery(), cancellationToken);

        var jsonData = JsonConvert.SerializeObject(userCache, Formatting.Indented);
        return Content(jsonData, "application/json", Encoding.UTF8);
    }

    [HttpGet("clear-user-cache")]
    public async Task<IActionResult> ClearCache(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ClearUserCacheCommand(), cancellationToken);

        return result.IsValid ? Content("Cache cleared") : Content("Failed to clear cache");
    }
}
