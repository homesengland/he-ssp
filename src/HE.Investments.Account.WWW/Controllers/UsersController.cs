using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.WWW.Models.Users;
using MediatR;
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

        return View("Index", (model, UserRoles.GetAll()));
    }

    [HttpGet("manage")]
    public async Task<IActionResult> Manage([FromQuery] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

        return View("Manage", model);
    }
}
