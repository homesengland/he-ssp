using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails;
using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.InvestmentLoans.Common.Routing;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{applicationId}/financial-details")]
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
        return View("Index", new FinancialDetailsBaseModel(applicationId, "Some application"));
    }

    [HttpPost("start")]
    [WorkflowState(FinancialDetailsWorkflowState.Index)]
    public async Task<IActionResult> StartPost(Guid applicationId, string applicationName, CancellationToken cancellationToken)
    {
        _ = await _mediator.Send(new StartFinancialDetailsCommand(ApplicationId.From(applicationId), applicationName), cancellationToken);

        return await Continue(new { applicationId });
    }

    [HttpGet("land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsLandStatusModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.PurchasePrice,
            financialDetails.IsPurchasePriceKnown ?? false));
    }

    [HttpPost("land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId, FinancialDetailsLandStatusModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvidePurchasePriceCommand(ApplicationId.From(applicationId), model.PurchasePrice, model.IsPurchasePriceKnown), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandStatus", model);
        }

        return await Continue(new { applicationId });
    }

    [HttpGet("land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsLandValueModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.LandValue,
            financialDetails.IsSchemaOnPublicLand));
    }

    [HttpPost("land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId, FinancialDetailsLandValueModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideLandOwnershipAndValueCommand(ApplicationId.From(applicationId), model.IsOnPublicLand, model.LandValue), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandValue", model);
        }

        return await Continue(new { applicationId });
    }

    [HttpGet("other-application-costs")]
    [WorkflowState(FinancialDetailsWorkflowState.OtherApplicationCosts)]
    public async Task<IActionResult> OtherApplicationCosts(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsOtherApplicationCostsModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.ExpectedWorkCost,
            financialDetails.ExpectedOnCost));
    }

    [HttpPost("other-application-costs")]
    [WorkflowState(FinancialDetailsWorkflowState.OtherApplicationCosts)]
    public async Task<IActionResult> OtherApplicationCosts(Guid applicationId, FinancialDetailsOtherApplicationCostsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideOtherApplicationCostsCommand(ApplicationId.From(applicationId), model.ExpectedWorksCosts, model.ExpectedOnCosts), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("OtherApplicationCosts", model);
        }

        return await Continue(new { applicationId });
    }

    [HttpGet("contributions")]
    [WorkflowState(FinancialDetailsWorkflowState.ExpectedContributions)]
    public async Task<IActionResult> Contributions(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsContributionsModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.RentalIncomeContribution,
            financialDetails.SubsidyFromSaleOnThisScheme,
            financialDetails.SubsidyFromSaleOnOtherSchemes,
            financialDetails.OwnResourcesContribution,
            financialDetails.RecycledCapitalGarntFundContribution,
            financialDetails.OtherCapitalContributions,
            financialDetails.InitialSalesReceiptContribution,
            financialDetails.TransferValueOfHomes,
            true, // temporarly mocked - shared ownership
            true)); // temporarly mocked - unregistered body account type
    }

    [HttpPost("contributions")]
    [WorkflowState(FinancialDetailsWorkflowState.ExpectedContributions)]
    public async Task<IActionResult> Contributions(Guid applicationId, FinancialDetailsContributionsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideContributionsCommand(
            ApplicationId.From(applicationId),
            model.RentalIncomeBorrowing,
            model.SaleOfHomesOnThisScheme,
            model.SaleOfHomesOnOtherSchemes,
            model.OwnResources,
            model.RCGFContribution,
            model.OtherCapitalSources,
            model.InitialSalesOfSharedHomes,
            model.HomesTransferValue),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Contributions", model);
        }

        return await Continue(new { applicationId });
    }

    [HttpGet("back")]
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
