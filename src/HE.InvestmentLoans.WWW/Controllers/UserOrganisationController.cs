using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.UserOrganisation.Queries;
using HE.InvestmentLoans.CRM.Model;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Models;
using HE.InvestmentLoans.WWW.Models.UserOrganisation;
using HE.InvestmentLoans.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

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
                userOrganisationResult.UserFirstName.Value,
                userOrganisationResult.IsLimitedUser,
                new List<ProgrammeToAccessModel>
                {
                    new(
                        ProgrammesConsts.LoansProgramme,
                        userOrganisationResult.LoanApplications.Select(a =>
                                new ApplicationBasicDetailsModel(a.Id.Value, a.ApplicationName, a.Status))
                            .ToList()),
                },
                new List<ProgrammeModel> { ProgrammesConsts.LoansProgramme },
                new List<ActionModel>
                {
                    new ActionModel($"Manage {userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName} details", "Details", "UserOrganisation"),
                    new ActionModel($"Manage your account", string.Empty, "Dashboard"),
                }));
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());

        return View("OrganizationDetails", organisationResult.OrganisationDetailsViewModel);
    }
}
