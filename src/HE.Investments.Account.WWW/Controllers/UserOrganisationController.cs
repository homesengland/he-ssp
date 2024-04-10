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
using HE.Investments.Common.WWW.Models;
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
                await UserOrganisationActions(userOrganisationResult.OrganisationBasicInformation.RegisteredCompanyName)));
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details()
    {
        var result = await _mediator.Send(new GetOrganisationDetailsQuery());

        return View("OrganisationDetails", result);
    }

    [HttpGet("request-details-change")]
    [AuthorizeWithCompletedProfile(UserRole.Admin)]
    public async Task<IActionResult> ChangeOrganisationDetails()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());
        if (organisationResult.ChangeRequestState == OrganisationChangeRequestState.NoPendingRequest)
        {
            return View(organisationResult);
        }

        return View("OrganisationDetails", organisationResult);
    }

    [HttpPost("request-details-change")]
    [AuthorizeWithCompletedProfile(UserRole.Admin)]
    public async Task<IActionResult> ChangeOrganisationDetails(OrganisationDetails model, CancellationToken cancellationToken)
    {
        var command = new ChangeOrganisationDetailsCommand(
            model.Name,
            model.PhoneNumber,
            model.AddressLine1,
            model.AddressLine2,
            model.TownOrCity,
            model.County,
            model.Postcode);

        return await this.ExecuteCommand<OrganisationDetails>(
            _mediator,
            command,
            () => Task.FromResult<IActionResult>(RedirectToAction("Details")),
            () => Task.FromResult<IActionResult>(View(model)),
            cancellationToken);
    }

    [HttpGet("list")]
    public IActionResult UserOrganisationsList()
    {
        return RedirectToAction("Index");
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
        return $"{_programmeUrlConfig.FrontDoor}/project/{projectId}";
    }

    private Dictionary<ProgrammeType, ProgrammeModel> GetProgrammes(IList<ProgrammeType> programmeTypes)
    {
        return programmeTypes.Select(x => _programmes.GetProgramme(x)).ToDictionary(x => x.Type, x => x);
    }

    private async Task<List<ActionModel>> UserOrganisationActions(string organisationName)
    {
        var canViewOrganisationDetails = await _accountAccessContext.CanAccessOrganisationView();
        var userOrganisationActions = new List<ActionModel>();
        if (canViewOrganisationDetails)
        {
            userOrganisationActions.AddRange(new List<ActionModel>
            {
                new("Add or manage users at this Organisation", "Index", "Users", HasAccess: canViewOrganisationDetails),
                new($"Manage {organisationName} details", "Details", "UserOrganisation", HasAccess: canViewOrganisationDetails),
            });
        }

        userOrganisationActions.Add(new("Manage your account", "GetProfileDetails", "User", new { callback = Url.Action("Index") }, true));
        return userOrganisationActions;
    }
}
