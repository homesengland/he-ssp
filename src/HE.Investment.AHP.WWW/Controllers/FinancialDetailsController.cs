using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Domain.FinancialDetails;
using HE.Investment.AHP.Domain.FinancialDetails.Commands;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Routing;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/financial-details")]
public class FinancialDetailsController : WorkflowController<FinancialDetailsWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly IFinancialDetailsSummaryViewModelFactory _financialDetailsSummaryViewModelFactory;

    public FinancialDetailsController(IMediator mediator, IFinancialDetailsSummaryViewModelFactory financialDetailsSummaryViewModelFactory)
    {
        _mediator = mediator;
        _financialDetailsSummaryViewModelFactory = financialDetailsSummaryViewModelFactory;
    }

    [HttpGet("start")]
    [WorkflowState(FinancialDetailsWorkflowState.Index)]
    public async Task<IActionResult> Start(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()), cancellationToken);
        return View("Index", new FinancialDetailsBaseModel(applicationId, application.Name));
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
        var siteLandStatus = false;

        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsLandStatusModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.PurchasePrice.ToPoundsPencesString(),
            financialDetails.IsPurchasePriceFinal ?? siteLandStatus));
    }

    [HttpPost("land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId, FinancialDetailsLandStatusModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideLandStatusCommand(ApplicationId.From(applicationId), model.PurchasePrice, model.IsFinal),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandStatus", model);
        }

        return await ContinueWithRedirect(new { applicationId });
    }

    [HttpGet("land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));

        var isSchemeOnPublicLand =
            financialDetails.IsSchemaOnPublicLand.HasValue
                ? financialDetails.IsSchemaOnPublicLand.Value ? CommonResponse.Yes : CommonResponse.No
                : string.Empty;

        return View(new FinancialDetailsLandValueModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.LandValue.ToPoundsPencesString(),
            isSchemeOnPublicLand));
    }

    [HttpPost("land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId, FinancialDetailsLandValueModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideLandValueCommand(ApplicationId.From(applicationId), model.IsOnPublicLand, model.LandValue),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandValue", model);
        }

        return await ContinueWithRedirect(new { applicationId });
    }

    [HttpGet("other-application-costs")]
    [WorkflowState(FinancialDetailsWorkflowState.OtherApplicationCosts)]
    public async Task<IActionResult> OtherApplicationCosts(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsOtherApplicationCostsModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.ExpectedWorkCost.ToWholeNumberString(),
            financialDetails.ExpectedOnCost.ToWholeNumberString()));
    }

    [HttpPost("other-application-costs")]
    [WorkflowState(FinancialDetailsWorkflowState.OtherApplicationCosts)]
    public async Task<IActionResult> OtherApplicationCosts(
        Guid applicationId,
        FinancialDetailsOtherApplicationCostsModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideOtherApplicationCostsCommand(ApplicationId.From(applicationId), model.ExpectedWorksCosts, model.ExpectedOnCosts),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("OtherApplicationCosts", model);
        }

        return await ContinueWithRedirect(new { applicationId });
    }

    [HttpGet("expected-contributions")]
    [WorkflowState(FinancialDetailsWorkflowState.Contributions)]
    public async Task<IActionResult> Contributions(Guid applicationId)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()));

        var isSharedOwnership = application.Tenure
            is Tenure.SharedOwnership
            or Tenure.HomeOwnershipLongTermDisabilities
            or Tenure.OlderPersonsSharedOwnership;

        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsContributionsModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.RentalIncomeContribution.ToString(),
            financialDetails.SubsidyFromSaleOnThisScheme.ToString(),
            financialDetails.SubsidyFromSaleOnOtherSchemes.ToString(),
            financialDetails.OwnResourcesContribution.ToString(),
            financialDetails.RecycledCapitalGarntFundContribution.ToString(),
            financialDetails.OtherCapitalContributions.ToString(),
            financialDetails.SharedOwnershipSalesContribution.ToString(),
            financialDetails.TransferValueOfHomes.ToString(),
            isSharedOwnership,
            true,
            financialDetails.TotalExpectedContributions.ToWholeNumberString()));
    }

    [HttpPost("expected-contributions")]
    [WorkflowState(FinancialDetailsWorkflowState.Contributions)]
    public async Task<IActionResult> Contributions(Guid applicationId, FinancialDetailsContributionsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideExpecteContributionsCommand(
                ApplicationId.From(applicationId),
                model.RentalIncomeBorrowing,
                model.SaleOfHomesOnThisScheme,
                model.SaleOfHomesOnOtherSchemes,
                model.OwnResources,
                model.RCGFContribution,
                model.OtherCapitalSources,
                model.SharedOwnershipSales,
                model.HomesTransferValue),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Contributions", model);
        }

        return await ContinueWithRedirect(new { applicationId });
    }

    [HttpGet("grants")]
    [WorkflowState(FinancialDetailsWorkflowState.Grants)]
    public async Task<IActionResult> Grants(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId.ToString()));
        return View(new FinancialDetailsGrantsModel(
            applicationId,
            financialDetails.ApplicationName,
            financialDetails.CountyCouncilGrants.ToString(),
            financialDetails.DHSCExtraCareGrants.ToString(),
            financialDetails.LocalAuthorityGrants.ToString(),
            financialDetails.SocialServicesGrants.ToString(),
            financialDetails.HealthRelatedGrants.ToString(),
            financialDetails.LotteryFunding.ToString(),
            financialDetails.OtherPublicGrants.ToString(),
            financialDetails.TotalRecievedGrands.ToWholeNumberString()));
    }

    [HttpPost("grants")]
    [WorkflowState(FinancialDetailsWorkflowState.Grants)]
    public async Task<IActionResult> Grants(Guid applicationId, FinancialDetailsGrantsModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideGrantsCommand(
                ApplicationId.From(applicationId),
                model.CountyCouncilGrants,
                model.DhscExtraCareGrants,
                model.LocalAuthorityGrants,
                model.SocialServicesGrants,
                model.HealthRelatedGrants,
                model.LotteryGrants,
                model.OtherPublicBodiesGrants),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Grants", model);
        }

        return await ContinueWithRedirect(new { applicationId });
    }

    [HttpGet("check-answers")]
    [WorkflowState(FinancialDetailsWorkflowState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid applicationId, CancellationToken cancellationToken)
    {
        var model = await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(applicationId.ToString(), Url, cancellationToken);

        return View(model);
    }

    [HttpPost("check-answers")]
    [WorkflowState(FinancialDetailsWorkflowState.CheckAnswers)]
    public async Task<IActionResult> Complete(Guid applicationId, FinancialDetailsCheckAnswersModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteFinancialDetailsCommand(ApplicationId.From(applicationId), model.IsSectionCompleted), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(
                "CheckAnswers",
                await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(applicationId.ToString(), Url, cancellationToken));
        }

        return RedirectToAction("TaskList", "Application", new { applicationId });
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(FinancialDetailsWorkflowState currentPage, Guid applicationId)
    {
        return Back(currentPage, new { applicationId });
    }

    protected override async Task<IStateRouting<FinancialDetailsWorkflowState>> Routing(FinancialDetailsWorkflowState currentState, object? routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                            ?? routeData?.GetPropertyValue<string>("applicationId")
                            ?? throw new NotFoundException($"{nameof(FinancialDetailsController)} required applicationId path parameter.");

        if (string.IsNullOrEmpty(applicationId))
        {
            throw new InvalidOperationException("Cannot find applicationId.");
        }

        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(applicationId));
        return new FinancialDetailsWorkflow(currentState, financialDetails);
    }
}
