using System.Globalization;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.HomeTypes.Queries;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Authorize]
[Route("Application/{applicationId}/HomeTypes")]
public class HomeTypesController : WorkflowController<HomeTypesWorkflowState>
{
    private readonly IMediator _mediator;

    public HomeTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [WorkflowState(HomeTypesWorkflowState.Index)]
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        return View(new HomeTypeModelBase(application.Name));
    }

    [HttpGet("Back")]
    public Task<IActionResult> Back([FromRoute] string applicationId, string homeTypeId, HomeTypesWorkflowState currentPage)
    {
        return Back(currentPage, new { applicationId, homeTypeId });
    }

    [WorkflowState(HomeTypesWorkflowState.List)]
    [HttpGet("List")]
    public async Task<IActionResult> List([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(applicationId), cancellationToken);

        return View(new HomeTypeListModel(application.Name)
        {
            HomeTypes = homeTypes.Select(x => new HomeTypeItemModel(x.Id, x.Name, x.HousingType, x.NumberOfHomes)).ToList(),
        });
    }

    [HttpGet("{homeTypeId}/Duplicate")]
    public async Task<IActionResult> Duplicate([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DuplicateHomeTypeCommand(applicationId, homeTypeId), cancellationToken);
        return RedirectToAction("List", new { applicationId });
    }

    [WorkflowState(HomeTypesWorkflowState.RemoveHomeType)]
    [HttpGet("{homeTypeId}/Remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);

        return View("RemoveHomeTypeConfirmation", new RemoveHomeTypeModel(application.Name, homeTypeDetails.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpGet("HomeTypeDetails")]
    public async Task<IActionResult> NewHomeTypeDetails([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        return View("HomeTypeDetails", new HomeTypeDetailsModel(application.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpPost("HomeTypeDetails")]
    public async Task<IActionResult> NewHomeTypeDetails([FromRoute] string applicationId, HomeTypeDetailsModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeDetails(applicationId, null, model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeTypeDetails)]
    [HttpGet("{homeTypeId}/HomeTypeDetails")]
    public async Task<IActionResult> HomeTypeDetails([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomeTypeDetailsModel(application.Name)
        {
            HomeTypeName = homeTypeDetails.Name,
            HousingType = homeTypeDetails.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeTypeDetails)]
    [HttpPost("{homeTypeId}/HomeTypeDetails")]
    public async Task<IActionResult> HomeTypeDetails([FromRoute] string applicationId, [FromRoute] string homeTypeId, HomeTypeDetailsModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeDetails(applicationId, homeTypeId, model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpGet("{homeTypeId}/HomeInformation")]
    public async Task<IActionResult> HomeInformation([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var homeInformation = await _mediator.Send(new GetHomeInformationQuery(applicationId, homeTypeId), cancellationToken);

        return View(new HomeInformationModel(application.Name, homeInformation.HomeTypeName)
        {
            NumberOfHomes = homeInformation.NumberOfHomes?.ToString(CultureInfo.InvariantCulture),
            NumberOfBedrooms = homeInformation.NumberOfBedrooms?.ToString(CultureInfo.InvariantCulture),
            MaximumOccupancy = homeInformation.MaximumOccupancy?.ToString(CultureInfo.InvariantCulture),
            NumberOfStoreys = homeInformation.NumberOfStoreys?.ToString(CultureInfo.InvariantCulture),
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpPost("{homeTypeId}/HomeInformation")]
    public async Task<IActionResult> HomeInformation([FromRoute] string applicationId, string homeTypeId, HomeInformationModel model, CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(
            new SaveHomeInformationCommand(
                applicationId,
                homeTypeId,
                model.NumberOfHomes,
                model.NumberOfBedrooms,
                model.MaximumOccupancy,
                model.NumberOfStoreys),
            model,
            cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpGet("{homeTypeId}/HomesForDisabledPeople")]
    public async Task<IActionResult> HomesForDisabledPeople([FromRoute] string applicationId, string homeTypeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var disabledHousingType = await _mediator.Send(new GetDisabledPeopleHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomesForDisabledPeopleModel(application.Name, disabledHousingType.HomeTypeName) { HousingType = disabledHousingType.HousingType });
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpPost("{homeTypeId}/HomesForDisabledPeople")]
    public async Task<IActionResult> HomesForDisabledPeople(
        [FromRoute] string applicationId,
        string homeTypeId,
        HomesForDisabledPeopleModel model,
        CancellationToken cancellationToken)
    {
        return await SaveHomeTypeSegment(new SaveDisabledPeopleHousingTypeCommand(applicationId, homeTypeId, model.HousingType), model, cancellationToken);
    }

    [WorkflowState(HomeTypesWorkflowState.DisablePeopleClientGroup)]
    [HttpGet("{homeTypeId}/DisabledPeopleClientGroup")]
    public IActionResult DisabledPeopleClientGroup(string homeTypeId)
    {
        return View();
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForOlderPeople)]
    [HttpGet("{homeTypeId}/HomesForOlder")]
    public IActionResult HomesForOlderPeople(string homeTypeId)
    {
        return View();
    }

    protected override async Task<IStateRouting<HomeTypesWorkflowState>> Routing(HomeTypesWorkflowState currentState, object routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                                ?? routeData?.GetPropertyValue<string>("applicationId")
                                ?? throw new NotFoundException($"{nameof(HomeTypesController)} required applicationId path parameter.");
        var homeTypeId = Request.GetRouteValue("homeTypeId") ?? routeData?.GetPropertyValue<string>("homeTypeId");
        if (string.IsNullOrEmpty(applicationId) || string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypesWorkflow();
        }

        var homeTypes = await _mediator.Send(new GetHomeTypeQuery(applicationId, homeTypeId));
        return new HomeTypesWorkflow(currentState, homeTypes);
    }

    private async Task<IActionResult> SaveHomeTypeDetails(
        string applicationId,
        string homeTypeId,
        HomeTypeDetailsModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SaveHomeTypeDetailsCommand(applicationId, homeTypeId, model.HomeTypeName, model.HousingType),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("HomeTypeDetails", model);
        }

        return await Continue(new { applicationId, homeTypeId = result.ReturnedData!.Value });
    }

    private async Task<IActionResult> SaveHomeTypeSegment<TModel, TSaveSegmentCommand>(
        TSaveSegmentCommand command,
        TModel model,
        CancellationToken cancellationToken)
        where TSaveSegmentCommand : SaveHomeTypeSegmentCommandBase
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationId = command.ApplicationId, homeTypeId = command.HomeTypeId });
    }
}
