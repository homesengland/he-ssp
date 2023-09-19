using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Funding;
using HE.InvestmentLoans.Contract.Funding.Commands;
using HE.InvestmentLoans.Contract.Funding.Enums;
using HE.InvestmentLoans.Contract.Funding.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application/{id}/funding")]
[AuthorizeWithCompletedProfile]
public class FundingV2Controller : WorkflowController<FundingState>
{
    private readonly IMediator _mediator;

    public FundingV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start-funding")]
    [WorkflowState(FundingState.Index)]
    public IActionResult StartFunding(Guid id)
    {
        return View("StartFunding", LoanApplicationId.From(id));
    }

    [HttpPost("start-funding")]
    [WorkflowState(FundingState.Index)]
    public async Task<IActionResult> StartFundingPost(Guid id)
    {
        return await Continue(new { Id = id });
    }

    [HttpGet("gross-development-value")]
    [WorkflowState(FundingState.GDV)]
    public async Task<IActionResult> GrossDevelopmentValue(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)));
        return View("GrossDevelopmentValue", response.ViewModel);
    }

    [HttpPost("gross-development-value")]
    [WorkflowState(FundingState.GDV)]
    public async Task<IActionResult> GrossDevelopmentValuePost(Guid id, FundingViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideGrossDevelopmentValueCommand(
                LoanApplicationId.From(id),
                viewModel.GrossDevelopmentValue),
            cancellationToken);

        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("GrossDevelopmentValue", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("estimated-total-costs")]
    [WorkflowState(FundingState.TotalCosts)]
    public async Task<IActionResult> EstimatedTotalCosts(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)));
        return View("EstimatedTotalCosts", response.ViewModel);
    }

    [HttpPost("estimated-total-costs")]
    [WorkflowState(FundingState.TotalCosts)]
    public async Task<IActionResult> EstimatedTotalCostsPost(Guid id, FundingViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideEstimatedTotalCostsCommand(
                LoanApplicationId.From(id),
                viewModel.TotalCosts),
            cancellationToken);

        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("EstimatedTotalCosts", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("abnormal-costs")]
    [WorkflowState(FundingState.AbnormalCosts)]
    public async Task<IActionResult> AbnormalCosts(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)));
        return View("AbnormalCosts", response.ViewModel);
    }

    [HttpPost("abnormal-costs")]
    [WorkflowState(FundingState.AbnormalCosts)]
    public async Task<IActionResult> AbnormalCostsPost(Guid id, FundingViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideAbnormalCostsCommand(
                LoanApplicationId.From(id),
                viewModel.AbnormalCosts,
                viewModel.AbnormalCostsInfo),
            cancellationToken);

        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("AbnormalCosts", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("private-sector-funding")]
    [WorkflowState(FundingState.PrivateSectorFunding)]
    public async Task<IActionResult> PrivateSectorFunding(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)));
        return View("PrivateSectorFunding", response.ViewModel);
    }

    [HttpPost("private-sector-funding")]
    [WorkflowState(FundingState.PrivateSectorFunding)]
    public async Task<IActionResult> PrivateSectorFundingPost(Guid id, FundingViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvidePrivateSectorFundingCommand(
                LoanApplicationId.From(id),
                viewModel.PrivateSectorFunding,
                viewModel.PrivateSectorFundingResult,
                viewModel.PrivateSectorFundingReason),
            cancellationToken);

        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("PrivateSectorFunding", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("repayment-system")]
    [WorkflowState(FundingState.Refinance)]
    public async Task<IActionResult> RepaymentSystem(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)));
        return View("RepaymentSystem", response.ViewModel);
    }

    [HttpPost("repayment-system")]
    [WorkflowState(FundingState.Refinance)]
    public async Task<IActionResult> RepaymentSystemPost(Guid id, FundingViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideRepaymentSystemCommand(
                LoanApplicationId.From(id),
                viewModel.Refinance,
                viewModel.RefinanceInfo),
            cancellationToken);

        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("RepaymentSystem", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("additional-projects")]
    [WorkflowState(FundingState.AdditionalProjects)]
    public async Task<IActionResult> AdditionalProjects(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)));
        return View("AdditionalProjects", response.ViewModel);
    }

    [HttpPost("additional-projects")]
    [WorkflowState(FundingState.AdditionalProjects)]
    public async Task<IActionResult> AdditionalProjectsPost(Guid id, FundingViewModel viewModel, [FromQuery] string redirect, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideAdditionalProjectsCommand(
                LoanApplicationId.From(id),
                viewModel.AdditionalProjects),
            cancellationToken);

        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("AdditionalProjects", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("check-answers")]
    [WorkflowState(FundingState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)), cancellationToken);
        return View("CheckAnswers", response.ViewModel);
    }

    [HttpPost("check-answers")]
    [WorkflowState(FundingState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswersPost(Guid id, FundingViewModel viewModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CheckAnswersFundingSectionCommand(LoanApplicationId.From(id), viewModel.CheckAnswers), cancellationToken);
        if (result.AreValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id)), cancellationToken);
            return View("CheckAnswers", response.ViewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(Guid id, FundingState currentPage)
    {
        return await Back(currentPage, new { Id = id });
    }

    protected override IStateRouting<FundingState> Routing(FundingState currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;

        var applicationId = !string.IsNullOrEmpty(id) ? LoanApplicationId.From(Guid.Parse(id)) : null;

        return new FundingWorkflow(applicationId, _mediator, currentState);
    }
}
