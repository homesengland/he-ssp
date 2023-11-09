using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails;
using HE.Investment.AHP.WWW.Models.Application;
using HE.InvestmentLoans.Common.Routing;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApplicationId = HE.Investment.AHP.Contract.FinancialDetails.ValueObjects.ApplicationId;

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
    public IActionResult Start(Guid applicationId, CancellationToken cancellationToken)
    {
        // temporarly mocked
        // var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()), cancellationToken);
        // return View("Index", new { applicationId, applicationName = application.Name });
        return View("Index", new ApplicationBasicModel(applicationId.ToString(), "Some application", string.Empty));
    }

    [HttpPost("start")]
    [WorkflowState(FinancialDetailsWorkflowState.Index)]
    public async Task<IActionResult> StartPost(Guid applicationId, string applicationName, CancellationToken cancellationToken)
    {
        _ = await _mediator.Send(new StartFinancialDetailsCommand(applicationId, applicationName), cancellationToken);

        return await Continue(new { applicationId });
    }

    [HttpGet("{applicationId}/land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(ApplicationId.From(applicationId)));
        return View(financialDetails);
    }

    [HttpPost("{applicationId}/land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId, FinancialDetailsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvidePurchasePriceCommand(ApplicationId.From(applicationId), model.PurchasePrice), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandStatus", model);
        }

        return await Continue(new { applicationId });
    }

    [HttpGet("{applicationId}/land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(ApplicationId.From(applicationId)));
        return View(financialDetails);
    }

    [HttpPost("{applicationId}/land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId, FinancialDetailsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideLandOwnershipAndValueCommand(ApplicationId.From(applicationId), model.IsSchemaOnPublicLand, model.LandValue), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandValue", model);
        }

        return await Continue(new { applicationId });
    }

    [HttpGet("{applicationId}/back")]
    public Task<IActionResult> Back(FinancialDetailsWorkflowState currentPage, Guid applicationId)
    {
        return Back(currentPage, new { applicationId });
    }

    protected override async Task<IStateRouting<FinancialDetailsWorkflowState>> Routing(FinancialDetailsWorkflowState currentState, object routeData = null)
    {
        var applicationId = await Task.Run(() => Request.GetRouteValue("applicationId") ?? routeData?.GetPropertyValue<Guid>("applicationId").ToString());
        if (string.IsNullOrEmpty(applicationId))
        {
            return new FinancialDetailsWorkflow();
        }

        return new FinancialDetailsWorkflow(currentState);
    }
}
