using HE.Investments.Account.Contract.UserOrganisations.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Models.UserOrganisations;
using HE.Investments.Common.WWW.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route(UserOrganisationsAccountEndpoints.Controller)]
[AuthorizeWithCompletedProfile]
public class UserOrganisationsController : Controller
{
    private readonly IMediator _mediator;

    public UserOrganisationsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(UserOrganisationsAccountEndpoints.ListSuffix)]
    public async Task<IActionResult> List()
    {
        var userOrganisationList = await _mediator.Send(new GetUserOrganisationListQuery());

        return View(
            "List",
            new UserOrganisationsListModel(
                userOrganisationList,
                CreateListViewActions()));
    }

    private List<ActionModel> CreateListViewActions()
    {
        return
        [
            new(
                "Add another organisation",
                "SearchOrganisation",
                "Organisation",
                new { callback = Url.Action("List", "UserOrganisations") },
                HasAccess: true,
                DataTestId: "add-another-organisation-link"),

            new(
                "Manage your account",
                "GetProfileDetails",
                "User",
                new { callback = Url.Action("List", "UserOrganisations") },
                HasAccess: true,
                DataTestId: "manage-profile-link")

        ];
    }
}
