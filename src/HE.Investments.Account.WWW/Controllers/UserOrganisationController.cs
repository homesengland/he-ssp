using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Contract.UserOrganisation.Commands;
using HE.Investments.Account.Contract.UserOrganisation.Queries;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Models.UserOrganisation;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route(UserOrganisationAccountEndpoints.Controller)]
[AuthorizeWithCompletedProfile]
public class UserOrganisationController : Controller
{
    private readonly IMediator _mediator;

    private readonly IProgrammes _programmes;

    private readonly IAccountAccessContext _accountAccessContext;

    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    public UserOrganisationController(
        IMediator mediator,
        IProgrammes programmes,
        IAccountAccessContext accountAccessContext,
        ProgrammeUrlConfig programmeUrlConfig)
    {
        _mediator = mediator;
        _programmes = programmes;
        _accountAccessContext = accountAccessContext;
        _programmeUrlConfig = programmeUrlConfig;
    }

    [HttpGet(UserOrganisationAccountEndpoints.DashboardSuffix)]
    public async Task<IActionResult> Index()
    {
        var userOrganisationResult = await _mediator.Send(new GetUserOrganisationWithProgrammesQuery());
        var canViewOrganisationDetails = await _accountAccessContext.CanAccessOrganisationView();
        var programmeModels = GetProgrammes(userOrganisationResult.ProgrammesToAccess.Select(x => x.Type).ToList());
        var projects = userOrganisationResult.Projects.Select(x => new UserProjectModel(x.Id.Value, x.Name, x.Status, GetProjectUrl(x.Id))).ToList();

        return View(
            "UserOrganisation",
            new UserOrganisationModel(
                userOrganisationResult.OrganisationBasicInformation.RegisteredCompanyName,
                userOrganisationResult.UserFirstName,
                userOrganisationResult.IsLimitedUser,
                await GetStartNewProjectUrl(),
                projects,
                userOrganisationResult.ProgrammesToAccess.Select(
                    p => new ProgrammeToAccessModel(
                        programmeModels[p.Type],
                        p.Applications.Select(a =>
                                new UserApplicationModel(
                                        a.Id.Value,
                                        a.ApplicationName,
                                        a.Status,
                                        _programmes.GetApplicationUrl(p.Type, a.Id)))
                            .ToList()))
                    .ToList(),
                new List<Common.WWW.Models.ActionModel>
                {
                    new("Add or manage users at this Organisation", "Index", "Users", HasAccess: canViewOrganisationDetails),
                    new($"Manage {userOrganisationResult.OrganisationBasicInformation.RegisteredCompanyName} details", "Details", "UserOrganisation", HasAccess: canViewOrganisationDetails),
                    new("Manage your account", "GetProfileDetails", "User", new { callback = Url.Action("Index") }, true),
                }));
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());

        return View("OrganisationDetails", organisationResult.OrganisationDetailsViewModel);
    }

    [HttpGet("request-details-change")]
    [AuthorizeWithCompletedProfile(UserRole.Admin)]
    public async Task<IActionResult> ChangeOrganisationDetails()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());
        if (organisationResult.OrganisationDetailsViewModel.ChangeRequestState == OrganisationChangeRequestState.NoPendingRequest)
        {
            return View(organisationResult.OrganisationDetailsViewModel);
        }

        return View("OrganisationDetails", organisationResult.OrganisationDetailsViewModel);
    }

    [HttpPost("request-details-change")]
    [AuthorizeWithCompletedProfile(UserRole.Admin)]
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

        return await this.ExecuteCommand<OrganisationDetailsViewModel>(
            _mediator,
            command,
            () => Task.FromResult<IActionResult>(RedirectToAction(
                nameof(Details),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix())),
            () => Task.FromResult<IActionResult>(View(viewModel)),
            cancellationToken);
    }

    private async Task<string?> GetStartNewProjectUrl()
    {
        if (await _accountAccessContext.CanEditApplication())
        {
            return $"{_programmeUrlConfig.FrontDoor}/project/start";
        }

        return null;
    }

    private string GetProjectUrl(HeProjectId projectId)
    {
        return $"{_programmeUrlConfig.FrontDoor}/project/{projectId}/name";
    }

    private Dictionary<ProgrammeType, ProgrammeModel> GetProgrammes(IList<ProgrammeType> programmeTypes)
    {
        return programmeTypes.Select(x => _programmes.GetProgramme(x)).ToDictionary(x => x.Type, x => x);
    }
}
