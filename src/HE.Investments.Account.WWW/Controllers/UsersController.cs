using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Contract.Users.Commands;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.WWW.Models.Users;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("users")]
[AuthorizeWithCompletedProfile(AccountAccessContext.OrganisationView)]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? page, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUsersAndJoinRequestsQuery(new PaginationRequest(page ?? 1)), cancellationToken);

        return View("Index", (model, UserRolesDescription.All));
    }

    [HttpGet("{id}/manage")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
    public async Task<IActionResult> Manage([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetUserDetailsQuery(UserGlobalId.From(id)), cancellationToken);

        return View("Manage", model);
    }

    [HttpPost("{id}/manage")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
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
            return RedirectToAction("ConfirmUnlink", "Users", new { id });
        }

        var result = await _mediator.Send(new ChangeUserRoleCommand(UserGlobalId.From(id), role.Value), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return await Manage(id, cancellationToken);
        }

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/admin-info")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
    public async Task<IActionResult> AdminInfo([FromRoute] string id, CancellationToken cancellationToken)
    {
        var model = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);

        return View("AdminInfo", (OrganisationName: model.OrganizationBasicInformation.RegisteredCompanyName, UserId: id));
    }

    [HttpGet("{id}/confirm-unlink")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
    public async Task<IActionResult> ConfirmUnlink([FromRoute] string id, CancellationToken cancellationToken)
    {
        var (organisationName, userDetails) = await _mediator.Send(new GetUserDetailsQuery(UserGlobalId.From(id)), cancellationToken);

        return View("ConfirmUnlink", (id, organisationName, $"{userDetails.FirstName} {userDetails.LastName}"));
    }

    [HttpPost("{id}/confirm-unlink")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
    public async Task<IActionResult> Unlink([FromRoute] string id, [FromForm] bool? unlink, CancellationToken cancellationToken)
    {
        if (unlink == null)
        {
            var (_, userDetails) = await _mediator.Send(new GetUserDetailsQuery(UserGlobalId.From(id)), cancellationToken);
            var userName = $"{userDetails.FirstName} {userDetails.LastName}".Trim();
            ModelState.AddModelError("Unlink", $"Select if you want to remove {userName} from the organisation");
            return await ConfirmUnlink(id, cancellationToken);
        }

        if (unlink.Value)
        {
            var result = await _mediator.Send(new RemoveLinkBetweenUserAndOrganisationCommand(UserGlobalId.From(id)), cancellationToken);

            if (result.HasValidationErrors)
            {
                ModelState.AddValidationErrors(result);
                return await ConfirmUnlink(id, cancellationToken);
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet("invite")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
    public async Task<IActionResult> Invite(CancellationToken cancellationToken)
    {
        var organisation = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);

        return View("Invite", new InviteUserViewModel(organisation.OrganizationBasicInformation.RegisteredCompanyName));
    }

    [HttpPost("invite")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.ManageUsers)]
    public async Task<IActionResult> Invite(InviteUserViewModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new InviteUserToOrganisationCommand(model.FirstName, model.LastName, model.EmailAddress, model.JobTitle, model.Role),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return RedirectToAction("Index");
    }
}
