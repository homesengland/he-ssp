using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("users")]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUsersAndJoinRequestsQuery(), cancellationToken);

        return View("Index", model);
    }

    [HttpGet("manage")]
    public IActionResult Manage([FromQuery] string id)
    {
        return View("Manage");
    }
}
