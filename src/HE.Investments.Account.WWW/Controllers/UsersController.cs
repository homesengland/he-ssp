using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.WWW.Models.Users;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.Enums;
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
            return await Manage(id, cancellationToken);
        }

        if (role == UserRole.Admin)
        {
            return RedirectToAction("AdminInfo", new { id });
        }

        if (role == UserRole.Undefined)
        {
            return RedirectToAction("ConfirmUnlink", "Users", new { id, redirect = "Manage" });
        }

        var result = await _mediator.Send(new ChangeUserRoleCommand(id, role.Value), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return await Manage(id, cancellationToken);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/admin-info")]
    public async Task<IActionResult> AdminInfo([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);

        return View("AdminInfo", (OrganisationName: model.OrganizationBasicInformation.RegisteredCompanyName, UserId: id));
    }

    [HttpGet("{id}/confirm-unlink")]
    public async Task<IActionResult> ConfirmUnlink([FromRoute] string id, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

        return View("ConfirmUnlink", (id, model.OrganisationName, $"{model.UserDetails.FirstName} {model.UserDetails.LastName}", redirect));
    }

    [HttpPost("{id}/confirm-unlink")]
    public async Task<IActionResult> Unlink([FromRoute] string id, [FromForm] string unlink, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(unlink))
        {
            ModelState.AddModelError("Unlink", ValidationErrorMessage.ChooseYourAnswer);
            return await ConfirmUnlink(id, redirect, cancellationToken);
        }

        if (unlink == YesNoAnswers.Yes.ToString())
        {
            var result = await _mediator.Send(new RemoveLinkBetweenUserAndOrganisationCommand(), cancellationToken);

            if (result.HasValidationErrors)
            {
                ModelState.AddValidationErrors(result);
                return await ConfirmUnlink(id, redirect, cancellationToken);
            }
        }

        return RedirectToAction(redirect, new { id });
    }
}
