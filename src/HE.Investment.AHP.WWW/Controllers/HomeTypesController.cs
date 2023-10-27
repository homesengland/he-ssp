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
[Route("{schemeId}/home-types")]
public class HomeTypesController : WorkflowController<HomeTypesWorkflowState>
{
    private readonly IMediator _mediator;

    public HomeTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/HomeTypes/back")]
    public Task<IActionResult> Back(string schemeId, string homeTypeId, HomeTypesWorkflowState currentPage)
    {
        return Back(currentPage, new { schemeId, homeTypeId });
    }

    [WorkflowState(HomeTypesWorkflowState.Index)]
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string schemeId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(schemeId), cancellationToken);
        return View(new HomeTypeModelBase(scheme.Name));
    }

    [WorkflowState(HomeTypesWorkflowState.List)]
    [HttpGet("list")]
    public async Task<IActionResult> List([FromRoute] string schemeId, CancellationToken cancellationToken)
    {
        return View(await FetchListModel(schemeId, cancellationToken));
    }

    [HttpPost("{homeTypeId}/duplicate")]
    public async Task<IActionResult> Duplicate([FromRoute] string schemeId, string homeTypeId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DuplicateHousingTypeCommand(schemeId, homeTypeId), cancellationToken);
        return View("List", await FetchListModel(schemeId, cancellationToken));
    }

    [WorkflowState(HomeTypesWorkflowState.HousingType)]
    [HttpGet("housing-type/{homeTypeId?}")]
    public async Task<IActionResult> HousingType([FromRoute] string schemeId, string homeTypeId, CancellationToken cancellationToken)
    {
        var financialScheme = await _mediator.Send(new GetFinancialSchemeQuery(schemeId), cancellationToken);
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return View(new HousingTypeModel(financialScheme.Name));
        }

        var housingType = await _mediator.Send(new GetHousingTypeSectionQuery(schemeId, homeTypeId), cancellationToken);
        return View(new HousingTypeModel(financialScheme.Name)
        {
            HomeTypeName = housingType.HomeTypeName,
            HousingType = housingType.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HousingType)]
    [HttpPost("housing-type/{homeTypeId?}")]
    public async Task<IActionResult> HousingType([FromRoute] string schemeId, HousingTypeModel model, string homeTypeId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SaveHousingTypeCommand(schemeId, homeTypeId, model.HomeTypeName, model.HousingType),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { schemeId, homeTypeId = result.ReturnedData!.Value });
    }

    [WorkflowState(HomeTypesWorkflowState.HomeInformation)]
    [HttpGet("home-information/{homeTypeId}")]
    public IActionResult HomeInformation(string homeTypeId)
    {
        return View();
    }

    [WorkflowState(HomeTypesWorkflowState.DisabledPeopleHousingType)]
    [HttpGet("disabled-housing-type/{homeTypeId}")]
    public IActionResult DisabledPeopleHousingType(string homeTypeId)
    {
        return View();
    }

    [WorkflowState(HomeTypesWorkflowState.OlderPeopleHousingType)]
    [HttpGet("older-housing-type/{homeTypeId}")]
    public IActionResult OlderPeopleHousingType(string homeTypeId)
    {
        return View();
    }

    protected override async Task<IStateRouting<HomeTypesWorkflowState>> Routing(HomeTypesWorkflowState currentState, object routeData = null)
    {
        var schemeId = Request.GetRouteValue("schemeId")
                                ?? routeData?.GetPropertyValue<string>("schemeId")
                                ?? throw new NotFoundException($"{nameof(HomeTypesController)} required schemeId path parameter.");
        var homeTypeId = Request.GetRouteValue("homeTypeId") ?? routeData?.GetPropertyValue<string>("homeTypeId");
        if (string.IsNullOrEmpty(schemeId) || string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypesWorkflow();
        }

        var homeTypes = await _mediator.Send(new GetHomeTypeQuery(schemeId, homeTypeId));
        return new HomeTypesWorkflow(currentState, homeTypes);
    }

    private async Task<HomeTypeListModel> FetchListModel(string schemeId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetFinancialSchemeQuery(schemeId), cancellationToken);
        var homeTypes = await _mediator.Send(new GetHomeTypesQuery(schemeId), cancellationToken);

        return new HomeTypeListModel(scheme.Name)
        {
            HomeTypes = homeTypes.Select(x => new HomeTypeItemModel(x.HomeTypeId, x.Name, x.HousingType, x.NumberOfHomes)).ToList(),
        };
    }
}
