using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.WWW.Models.Users;
using HE.Investments.Common.Validators;
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

        return View("Index", (model, UserRoles.All));
    }

    [HttpGet("{id}/manage")]
    public async Task<IActionResult> Manage([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

        return View("Manage", model);
    }

    [HttpPost("{id}/manage")]
    public async Task<IActionResult> ChangeRole([FromRoute] string id, [FromForm] UserRole? role, CancellationToken cancellationToken)
    {
        if (role == null)
        {
            ModelState.AddModelError("Role", "You have to select role.");
            var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

            return View("Manage", model);
        }

        OperationResult result;
        if (role == UserRole.Admin)
        {
            return RedirectToAction("AdminInfo", new { id });
        }

        if (role == UserRole.Undefined)
        {
            result = await _mediator.Send(new RemoveLinkBetweenUserAndOrganisationCommand(), cancellationToken);
        }
        else
        {
            result = await _mediator.Send(new ChangeUserRoleCommand(id, role.Value), cancellationToken);
        }

        if (result.HasValidationErrors)
        {
            var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

            ModelState.AddValidationErrors(result);
            return View("Manage", model);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/admin-info")]
    public async Task<IActionResult> AdminInfo([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);

        return View("AdminInfo", (OrganisationName: model.OrganizationBasicInformation.RegisteredCompanyName, UserId: id));
    }
}
