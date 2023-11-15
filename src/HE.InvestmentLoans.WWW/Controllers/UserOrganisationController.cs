using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.WWW.Models.UserOrganisation;
using HE.InvestmentLoans.WWW.Utils;
using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("user-organisation")]
[AuthorizeWithCompletedProfile]
public class UserOrganisationController : BaseController
{
    private readonly IMediator _mediator;

    public UserOrganisationController(IMediator mediator)
        : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userOrganisationResult = await _mediator.Send(new GetUserOrganisationInformationQuery());
        var userApplications = userOrganisationResult.ProgrammesToAccess
            .FirstOrDefault(p => p.Type == ProgrammeType.Loans)
            ?.Applications ?? new List<UserApplication>();

        return View(
            "UserOrganisation",
            new UserOrganisationModel(
                userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName,
                userOrganisationResult.UserFirstName,
                userOrganisationResult.IsLimitedUser,
                new List<ProgrammeToAccessModel>
                {
                    new(
                        ProgrammesConsts.LoansProgramme,
                        userApplications.Select(a =>
                                new ApplicationBasicDetailsModel(Guid.Parse(a.Id), a.ApplicationName, a.Status))
                            .ToList()),
                },
                new List<ProgrammeModel> { ProgrammesConsts.LoansProgramme },
                new List<ActionModel>
                {
                    new ActionModel(
                        $"Manage {userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName} details",
                        "Details",
                        "UserOrganisation"),
                    new ActionModel($"Manage your account", string.Empty, "Dashboard"),
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

        return await ExecuteCommand(
            command,
            () => RedirectToAction(
                nameof(UserOrganisationController.Details),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix()),
            () => View(viewModel),
            cancellationToken);
    }
}
