using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.WWW.Models.Users;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("users")]
[AuthorizeWithCompletedProfile(UserAccountRole.AccessOrganisationRoles)]
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

        return View("Index", (model, UserRolesDescription.All));
    }

    [HttpGet("{id}/change")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> Change([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

        return View("Change", model);
    }

    [HttpPost("{id}/change")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> ChangeRole([FromRoute] string id, [FromForm] UserRole? role, CancellationToken cancellationToken)
    {
        if (role == null)
        {
            ModelState.AddModelError("Role", "You have to select role.");
            return await Change(id, cancellationToken);
        }

        if (role == UserRole.Admin)
        {
            return RedirectToAction("AdminInfo", new { id });
        }

        if (role == UserRole.Undefined)
        {
            return RedirectToAction("ConfirmUnlink", "Users", new { id });
        }

        var result = await _mediator.Send(new ChangeUserRoleCommand(id, role.Value), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return await Change(id, cancellationToken);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/admin-info")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> AdminInfo([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);

        return View("AdminInfo", (OrganisationName: model.OrganizationBasicInformation.RegisteredCompanyName, UserId: id));
    }

    [HttpGet("{id}/confirm-unlink")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> ConfirmUnlink([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUserDetailsQuery(id), cancellationToken);

        return View("ConfirmUnlink", (id, model.OrganisationName, $"{model.UserDetails.FirstName} {model.UserDetails.LastName}"));
    }

    [HttpPost("{id}/confirm-unlink")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> Unlink([FromRoute] string id, [FromForm] string? unlink, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(unlink))
        {
            ModelState.AddModelError("Unlink", ValidationErrorMessage.ChooseYourAnswer);
            return await ConfirmUnlink(id, cancellationToken);
        }

        if (unlink == YesNoAnswers.Yes.ToString())
        {
            var result = await _mediator.Send(new RemoveLinkBetweenUserAndOrganisationCommand(id), cancellationToken);

            if (result.HasValidationErrors)
            {
                ModelState.AddValidationErrors(result);
                return await ConfirmUnlink(id, cancellationToken);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet("invite")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> Invite(CancellationToken cancellationToken)
    {
        var organisation = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);

        return View("Invite", new InviteUserViewModel(organisation.OrganizationBasicInformation.RegisteredCompanyName));
    }

    [HttpPost("invite")]
    [AuthorizeWithCompletedProfile(UserAccountRole.AdminRole)]
    public async Task<IActionResult> Invite(InviteUserViewModel model, CancellationToken cancellationToken)
    {
        ModelState.AddModelError("FirstName", "test error");
        ModelState.AddModelError("Role", "role error");
        return await Invite(cancellationToken);

        // TODO: Uncommet when backend inplemented
        // var result = await _mediator.Send(new InviteUserCommand(model), cancellationToken);
        // if (result.HasValidationErrors)
        // {
        //     ModelState.AddValidationErrors(result);
        //     return View("Invite", model);
        // }
        //
        // return RedirectToAction("Index");
    }
}
