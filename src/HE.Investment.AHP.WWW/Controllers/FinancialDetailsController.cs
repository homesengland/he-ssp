using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investment.AHP.Domain.FinancialDetails;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("financial-details")]
public class FinancialDetailsController : WorkflowController<FinancialDetailsWorkflowState>
{
    private readonly IMediator _mediator;

    public FinancialDetailsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start")]
    [WorkflowState(FinancialDetailsWorkflowState.Index)]
    public IActionResult Start(Guid applicationId)
    {
        return View("Index", applicationId);
    }

    [HttpPost("start")]
    [WorkflowState(FinancialDetailsWorkflowState.Index)]
    public async Task<IActionResult> StartPost([FromQuery] Guid applicationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new StartFinancialDetailsCommand(applicationId), cancellationToken);

        return await Continue(new { applicationId, financialDetailsId = result.ReturnedData.FinancialDetailsId.ToString() });
    }

    [HttpGet("{financialDetailsId}/land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId, Guid financialDetailsId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(FinancialDetailsId.From(financialDetailsId)));
        return View(financialDetails);
    }

    [HttpPost("{financialDetailsId}/land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId, Guid financialDetailsId, FinancialDetailsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvidePurchasePriceCommand(FinancialDetailsId.From(financialDetailsId), model.PurchasePrice), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandStatus", model);
        }

        return await Continue(new { applicationId, financialDetailsId });
    }

    [HttpGet("{financialDetailsId}/land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId, Guid financialDetailsId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(FinancialDetailsId.From(financialDetailsId)));
        return View(financialDetails);
    }

    [HttpPost("{financialDetailsId}/land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid id, FinancialDetailsModel model)
    {
        var result = await _mediator.Send(new ProvideLandOwnershipAndValueCommand(FinancialDetailsId.From(id), model.IsSchemaOnPublicLand, model.LandValue));

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandValue", model);
        }

        return View("OtherSchemeCost", id);
    }

    [HttpGet("{financialDetailsId}/back")]
    public Task<IActionResult> Back(FinancialDetailsWorkflowState currentPage, Guid id)
    {
        return Back(currentPage, new { id });
    }

    protected override async Task<IStateRouting<FinancialDetailsWorkflowState>> Routing(FinancialDetailsWorkflowState currentState, object routeData = null)
    {
        var financialDetailsId = await Task.Run(() => Request.GetRouteValue("financialDetailsId") ?? routeData?.GetPropertyValue<string>("financialDetailsId"));
        if (string.IsNullOrEmpty(financialDetailsId))
        {
            return new FinancialDetailsWorkflow();
        }

        return new FinancialDetailsWorkflow(currentState);
    }
}
