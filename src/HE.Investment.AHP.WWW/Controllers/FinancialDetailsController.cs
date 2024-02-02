using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investment.AHP.WWW.Models.FinancialDetails.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/financial-details")]
public class FinancialDetailsController : WorkflowController<FinancialDetailsWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly IFinancialDetailsSummaryViewModelFactory _financialDetailsSummaryViewModelFactory;
    private readonly IAccountAccessContext _accountAccessContext;

    public FinancialDetailsController(
        IMediator mediator,
        IFinancialDetailsSummaryViewModelFactory financialDetailsSummaryViewModelFactory,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _financialDetailsSummaryViewModelFactory = financialDetailsSummaryViewModelFactory;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("start")]
    [WorkflowState(FinancialDetailsWorkflowState.Index)]
    public async Task<IActionResult> Start(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        return View("Index", new FinancialDetailsBaseModel(applicationId, application.Name));
    }

    [HttpGet("land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId)
    {
        var siteLandStatus = false;

        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(AhpApplicationId.From(applicationId)));
        return View(new FinancialDetailsLandStatusModel(
            applicationId,
            financialDetails.ApplicationName,
            CurrencyHelper.InputPoundsPences(financialDetails.PurchasePrice),
            financialDetails.IsPurchasePriceFinal ?? siteLandStatus));
    }

    [HttpPost("land-status")]
    [WorkflowState(FinancialDetailsWorkflowState.LandStatus)]
    public async Task<IActionResult> LandStatus(Guid applicationId, FinancialDetailsLandStatusModel model, CancellationToken cancellationToken)
    {
        return await ProvideFinancialDetails(
            new ProvideLandStatusCommand(AhpApplicationId.From(applicationId), model.PurchasePrice, model.IsFinal),
            model,
            cancellationToken);
    }

    [HttpGet("land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(AhpApplicationId.From(applicationId)));
        return View(new FinancialDetailsLandValueModel(
            applicationId,
            financialDetails.ApplicationName,
            CurrencyHelper.InputPoundsPences(financialDetails.LandValue),
            financialDetails.IsSchemaOnPublicLand));
    }

    [HttpPost("land-value")]
    [WorkflowState(FinancialDetailsWorkflowState.LandValue)]
    public async Task<IActionResult> LandValue(Guid applicationId, FinancialDetailsLandValueModel model, CancellationToken cancellationToken)
    {
        return await ProvideFinancialDetails(
            new ProvideLandValueCommand(AhpApplicationId.From(applicationId), model.IsOnPublicLand, model.LandValue),
            model,
            cancellationToken);
    }

    [HttpGet("other-application-costs")]
    [WorkflowState(FinancialDetailsWorkflowState.OtherApplicationCosts)]
    public async Task<IActionResult> OtherApplicationCosts(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(AhpApplicationId.From(applicationId)));
        return View(new FinancialDetailsOtherApplicationCostsModel(
            applicationId,
            financialDetails.ApplicationName,
            CurrencyHelper.InputPounds(financialDetails.ExpectedWorkCost),
            CurrencyHelper.InputPounds(financialDetails.ExpectedOnCost)));
    }

    [HttpPost("other-application-costs")]
    [WorkflowState(FinancialDetailsWorkflowState.OtherApplicationCosts)]
    public async Task<IActionResult> OtherApplicationCosts(
        Guid applicationId,
        FinancialDetailsOtherApplicationCostsModel model,
        CancellationToken cancellationToken)
    {
        return await ProvideFinancialDetails(
            new ProvideOtherApplicationCostsCommand(AhpApplicationId.From(applicationId), model.ExpectedWorksCosts, model.ExpectedOnCosts),
            model,
            cancellationToken);
    }

    [HttpGet("expected-contributions")]
    [WorkflowState(FinancialDetailsWorkflowState.Contributions)]
    public async Task<IActionResult> Contributions(Guid applicationId)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)));

        var isSharedOwnership = application.Tenure
            is Tenure.SharedOwnership
            or Tenure.HomeOwnershipLongTermDisabilities
            or Tenure.OlderPersonsSharedOwnership;

        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(AhpApplicationId.From(applicationId)));
        return View(new FinancialDetailsContributionsModel(
            applicationId,
            financialDetails.ApplicationName,
            CurrencyHelper.InputPounds(financialDetails.RentalIncomeContribution),
            CurrencyHelper.InputPounds(financialDetails.SubsidyFromSaleOnThisScheme),
            CurrencyHelper.InputPounds(financialDetails.SubsidyFromSaleOnOtherSchemes),
            CurrencyHelper.InputPounds(financialDetails.OwnResourcesContribution),
            CurrencyHelper.InputPounds(financialDetails.RecycledCapitalGrantFundContribution),
            CurrencyHelper.InputPounds(financialDetails.OtherCapitalContributions),
            CurrencyHelper.InputPounds(financialDetails.SharedOwnershipSalesContribution),
            CurrencyHelper.InputPounds(financialDetails.TransferValueOfHomes),
            isSharedOwnership,
            true,
            CurrencyHelper.DisplayPounds(financialDetails.TotalExpectedContributions)));
    }

    [HttpPost("expected-contributions")]
    [WorkflowState(FinancialDetailsWorkflowState.Contributions)]
    public async Task<IActionResult> Contributions(
        Guid applicationId,
        FinancialDetailsContributionsModel model,
        string action,
        CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateExpectedContributionsQuery(
                    AhpApplicationId.From(applicationId),
                    model.RentalIncomeBorrowing,
                    model.SaleOfHomesOnThisScheme,
                    model.SaleOfHomesOnOtherSchemes,
                    model.OwnResources,
                    model.RCGFContribution,
                    model.OtherCapitalSources,
                    model.SharedOwnershipSales,
                    model.HomesTransferValue),
                cancellationToken);

            model.TotalExpectedContributions = CurrencyHelper.DisplayPoundsPences(calculationResult.TotalExpectedContributions);

            ModelState.AddValidationErrors(operationResult);

            return View(model);
        }

        return await ProvideFinancialDetails(
            new ProvideExpectedContributionsCommand(
                AhpApplicationId.From(applicationId),
                model.RentalIncomeBorrowing,
                model.SaleOfHomesOnThisScheme,
                model.SaleOfHomesOnOtherSchemes,
                model.OwnResources,
                model.RCGFContribution,
                model.OtherCapitalSources,
                model.SharedOwnershipSales,
                model.HomesTransferValue),
            model,
            cancellationToken);
    }

    [HttpGet("grants")]
    [WorkflowState(FinancialDetailsWorkflowState.Grants)]
    public async Task<IActionResult> Grants(Guid applicationId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(AhpApplicationId.From(applicationId)));
        return View(new FinancialDetailsGrantsModel(
            applicationId,
            financialDetails.ApplicationName,
            CurrencyHelper.InputPounds(financialDetails.CountyCouncilGrants),
            CurrencyHelper.InputPounds(financialDetails.DhscExtraCareGrants),
            CurrencyHelper.InputPounds(financialDetails.LocalAuthorityGrants),
            CurrencyHelper.InputPounds(financialDetails.SocialServicesGrants),
            CurrencyHelper.InputPounds(financialDetails.HealthRelatedGrants),
            CurrencyHelper.InputPounds(financialDetails.LotteryFunding),
            CurrencyHelper.InputPounds(financialDetails.OtherPublicGrants),
            CurrencyHelper.DisplayPounds(financialDetails.TotalReceivedGrants)));
    }

    [HttpPost("grants")]
    [WorkflowState(FinancialDetailsWorkflowState.Grants)]
    public async Task<IActionResult> Grants(Guid applicationId, FinancialDetailsGrantsModel model, string action, CancellationToken cancellationToken)
    {
        if (action == GenericMessages.Calculate)
        {
            var (operationResult, calculationResult) = await _mediator.Send(
                new CalculateGrantsQuery(
                    AhpApplicationId.From(applicationId),
                    model.CountyCouncilGrants,
                    model.DhscExtraCareGrants,
                    model.LocalAuthorityGrants,
                    model.SocialServicesGrants,
                    model.HealthRelatedGrants,
                    model.LotteryGrants,
                    model.OtherPublicBodiesGrants),
                cancellationToken);

            model.TotalGrants = CurrencyHelper.DisplayPoundsPences(calculationResult.TotalReceivedGrants);

            ModelState.AddValidationErrors(operationResult);

            return View(model);
        }

        return await ProvideFinancialDetails(
            new ProvideGrantsCommand(
                AhpApplicationId.From(applicationId),
                model.CountyCouncilGrants,
                model.DhscExtraCareGrants,
                model.LocalAuthorityGrants,
                model.SocialServicesGrants,
                model.HealthRelatedGrants,
                model.LotteryGrants,
                model.OtherPublicBodiesGrants),
            model,
            cancellationToken);
    }

    [HttpGet("check-answers")]
    [WorkflowState(FinancialDetailsWorkflowState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid applicationId, CancellationToken cancellationToken)
    {
        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        var model = await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(
            AhpApplicationId.From(applicationId),
            Url,
            isReadOnly,
            cancellationToken);

        return View(model);
    }

    [HttpPost("check-answers")]
    [WorkflowState(FinancialDetailsWorkflowState.CheckAnswers)]
    public async Task<IActionResult> Complete(Guid applicationId, FinancialDetailsCheckAnswersModel model, string action, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteFinancialDetailsCommand(AhpApplicationId.From(applicationId), model.IsSectionCompleted), cancellationToken);

        if (result.HasValidationErrors)
        {
            var isReadOnly = !await _accountAccessContext.CanEditApplication();
            var summary = await _financialDetailsSummaryViewModelFactory.GetFinancialDetailsAndCreateSummary(
                AhpApplicationId.From(applicationId),
                Url,
                isReadOnly,
                cancellationToken);

            ModelState.AddValidationErrors(result);
            return View("CheckAnswers", summary);
        }

        return RedirectToAction(
            nameof(ApplicationController.TaskList),
            new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
            new { model.ApplicationId });
    }

    [HttpGet]
    [WorkflowState(FinancialDetailsWorkflowState.ReturnToTaskList)]
    public IActionResult ReturnToTaskList(Guid applicationId, CancellationToken cancellationToken)
    {
        return RedirectToAction(
                nameof(ApplicationController.TaskList),
                new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
                new { ApplicationId = applicationId });
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(FinancialDetailsWorkflowState currentPage, Guid applicationId)
    {
        return await Back(currentPage, new { applicationId });
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

        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(AhpApplicationId.From(applicationId)));
        var isReadOnly = !await _accountAccessContext.CanEditApplication() || financialDetails.IsReadOnly;
        return new FinancialDetailsWorkflow(currentState, financialDetails, isReadOnly);
    }

    private async Task<IActionResult> ProvideFinancialDetails<TModel, TCommand>(
        TCommand command,
        TModel model,
        CancellationToken cancellationToken)
        where TCommand : IRequest<OperationResult>
        where TModel : FinancialDetailsBaseModel
    {
        var applicationId = this.GetApplicationIdFromRoute();
        return await this.ExecuteCommand<TModel>(
            _mediator,
            command,
            async () => await ContinueWithAllRedirects(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View(model)),
            cancellationToken);
    }

    private async Task<IActionResult> ContinueWithAllRedirects(object routeData)
    {
        var action = HttpContext.Request.Form["action"];
        if (action == GenericMessages.SaveAndReturn)
        {
            var applicationId = this.GetApplicationIdFromRoute();
            return Url.RedirectToTaskList(applicationId.Value);
        }

        return await ContinueWithRedirect(routeData);
    }
}
