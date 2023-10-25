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
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{financialSchemeId}/home-types")]
public class HomeTypesController : WorkflowController<HomeTypesWorkflowState>
{
    private readonly IMediator _mediator;

    public HomeTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [WorkflowState(HomeTypesWorkflowState.Index)]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/HomeTypes/back")]
    public Task<IActionResult> Back(string financialSchemeId, string homeTypeId, HomeTypesWorkflowState currentPage)
    {
        return Back(currentPage, new { financialSchemeId, homeTypeId });
    }

    [WorkflowState(HomeTypesWorkflowState.HousingType)]
    [HttpGet("housing-type/{homeTypeId?}")]
    public async Task<IActionResult> HousingType([FromRoute] string financialSchemeId, string homeTypeId, CancellationToken cancellationToken)
    {
        var financialScheme = await _mediator.Send(new GetFinancialSchemeQuery(financialSchemeId), cancellationToken);
        if (string.IsNullOrEmpty(homeTypeId))
        {
            return View(new HousingTypeModel(financialScheme.Name));
        }

        var housingType = await _mediator.Send(new GetHousingTypeSectionQuery(financialSchemeId, homeTypeId), cancellationToken);
        return View(new HousingTypeModel(financialScheme.Name)
        {
            HomeTypeName = housingType.HomeTypeName,
            HousingType = housingType.HousingType,
        });
    }

    [WorkflowState(HomeTypesWorkflowState.HousingType)]
    [HttpPost("housing-type/{homeTypeId?}")]
    public async Task<IActionResult> HousingType([FromRoute] string financialSchemeId, HousingTypeModel model, string homeTypeId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SaveHousingTypeCommand(financialSchemeId, homeTypeId, model.HomeTypeName, model.HousingType),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { financialSchemeId, homeTypeId = result.ReturnedData.Value });
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
        var financialSchemeId = Request.GetRouteValue("financialSchemeId")
                                ?? routeData?.GetPropertyValue<string>("financialSchemeId")
                                ?? throw new NotFoundException($"{nameof(HomeTypesController)} required financialSchemeId path parameter.");
        var homeTypeId = Request.GetRouteValue("homeTypeId") ?? routeData?.GetPropertyValue<string>("homeTypeId");
        if (string.IsNullOrEmpty(financialSchemeId) || string.IsNullOrEmpty(homeTypeId))
        {
            return new HomeTypesWorkflow();
        }

        var homeTypes = await _mediator.Send(new GetHomeTypeQuery(financialSchemeId, homeTypeId));
        return new HomeTypesWorkflow(currentState, homeTypes);
    }
}
