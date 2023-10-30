using HE.Investment.AHP.Contract.Finance.Queries;
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
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(applicationId), cancellationToken);
        return View(new HomeTypeModelBase(scheme.Name));
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
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(applicationId), cancellationToken);
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(applicationId), cancellationToken);

        return View(new HomeTypeListModel(scheme.Name)
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
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(applicationId), cancellationToken);
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);

        return View("RemoveHomeTypeConfirmation", new RemoveHomeTypeModel(scheme.Name, homeTypeDetails.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.NewHomeTypeDetails)]
    [HttpGet("HomeTypeDetails")]
    public async Task<IActionResult> NewHomeTypeDetails([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(applicationId), cancellationToken);
        return View("HomeTypeDetails", new HomeTypeDetailsModel(scheme.Name));
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
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(applicationId), cancellationToken);
        var homeTypeDetails = await _mediator.Send(new GetHomeTypeDetailsQuery(applicationId, homeTypeId), cancellationToken);
        return View(new HomeTypeDetailsModel(scheme.Name)
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
    public IActionResult HomeInformation(string homeTypeId)
    {
        return View();
    }

    [WorkflowState(HomeTypesWorkflowState.HomesForDisabledPeople)]
    [HttpGet("{homeTypeId}/HomesForDisabled")]
    public IActionResult HomesForDisabledPeople(string homeTypeId)
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
            return View(model);
        }

        return await Continue(new { applicationId, homeTypeId = result.ReturnedData!.Value });
    }
}
