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

    public UserOrganisationController(IMediator mediator, IProgrammes programmes, IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _programmes = programmes;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet(UserOrganisationAccountEndpoints.DashboardSuffix)]
    public async Task<IActionResult> Index()
    {
        var userOrganisationResult = await _mediator.Send(new GetUserOrganisationWithProgrammesQuery());
        var canViewOrganisationDetails = await _accountAccessContext.CanAccessOrganisationView();
        var programmeModels = await GetProgrammes(
            userOrganisationResult.ProgrammesTypesToApply.Concat(userOrganisationResult.ProgrammesToAccess.Select(x => x.Type)).Distinct().ToList());

        return View(
            "UserOrganisation",
            new UserOrganisationModel(
                userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName,
                userOrganisationResult.UserFirstName,
                userOrganisationResult.IsLimitedUser,
                userOrganisationResult.ProgrammesToAccess.Select(
                    p => new ProgrammeToAccessModel(
                        programmeModels[p.Type],
                        p.Applications.Select(a =>
                                new ApplicationBasicDetailsModel(
                                        a.Id.Value,
                                        a.ApplicationName,
                                        a.Status,
                                        _programmes.GetApplicationUrl(p.Type, a.Id)))
                            .ToList()))
                    .ToList(),
                userOrganisationResult.ProgrammesTypesToApply.Select(t => programmeModels[t]).ToList(),
                new List<Common.WWW.Models.ActionModel>
                {
                    new($"Manage {userOrganisationResult.OrganizationBasicInformation.RegisteredCompanyName} details", "Details", "UserOrganisation", HasAccess: canViewOrganisationDetails),
                    new("Add or manage users at this Organisation", "Index", "Users", HasAccess: canViewOrganisationDetails),
                    new("Manage your account", "GetProfileDetails", "User", new { callback = Url.Action("Index") }, true),
                }));
    }

    [HttpGet("details")]
    public async Task<IActionResult> Details()
    {
        var organisationResult = await _mediator.Send(new GetOrganisationDetailsQuery());

        return View("OrganizationDetails", organisationResult.OrganisationDetailsViewModel);
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

        return View("OrganizationDetails", organisationResult.OrganisationDetailsViewModel);
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

    private async Task<Dictionary<ProgrammeType, ProgrammeModel>> GetProgrammes(IList<ProgrammeType> programmeTypes)
    {
        var programmes = await Task.WhenAll(programmeTypes.Select(x => _programmes.GetProgramme(x)));

        return programmeTypes.Zip(programmes).ToDictionary(x => x.First, x => x.Second);
    }
}
