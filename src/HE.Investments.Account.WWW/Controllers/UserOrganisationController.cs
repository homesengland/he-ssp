using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.WWW.Controllers.Consts;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("user-organisation")]
[AuthorizeWithCompletedProfile]
public class UserOrganisationController : Controller
{
    private readonly IMediator _mediator;

    public UserOrganisationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userOrganisationResult = await _mediator.Send(new GetUserOrganisationInformationQuery());
        return View(
            "UserOrganisation",
            new UserOrganisationModel(
                userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName,
                userOrganisationResult.UserFirstName,
                userOrganisationResult.IsLimitedUser,
                userOrganisationResult.ProgrammesToAccess.Select(p => new ProgrammeToAccessModel(
                        ProgrammesConsts.GetByType(p.Type),
                        p.Applications.Select(a =>
                                new ApplicationBasicDetailsModel(a.Id, a.ApplicationName, a.Status))
                            .ToList()))
                    .ToList(),
                userOrganisationResult.ProgrammesTypesToApply.Select(t => ProgrammesConsts.GetByType(t)).ToList(),
                new List<Common.WWW.Models.ActionModel>
                {
                    new($"Manage {userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName} details", "Details", "UserOrganisation"),
                    new("Manage your account", string.Empty, "Dashboard"),
                }));
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());

        return View("OrganizationDetails", organisationResult.OrganisationDetailsViewModel);
    }

    [HttpGet("request-details-change")]
    public async Task<IActionResult> ChangeOrganisationDetails()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());
        if (organisationResult.OrganisationDetailsViewModel.ChangeRequestState == OrganisationChangeRequestState.NoPendingRequest)
        {
            return View(organisationResult.OrganisationDetailsViewModel);
        }

        return View("OrganizationDetails", organisationResult.OrganisationDetailsViewModel);
    }

    [HttpPost("request-details-change")]
    public async Task<IActionResult> ChangeOrganisationDetails(OrganisationDetailsViewModel viewModel, CancellationToken cancellationToken)
    {
        var command = new ChangeOrganisationDetailsCommand(
            viewModel.Name,
            viewModel.PhoneNumber,
            viewModel.AddressLine1,
            viewModel.AddressLine2,
            viewModel.TownOrCity,
            viewModel.County,
            viewModel.Postcode);

        return await this.ExecuteCommand(
            _mediator,
            command,
            () => RedirectToAction(
                nameof(Details),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix()),
            () => View(viewModel),
            cancellationToken);
    }
}
