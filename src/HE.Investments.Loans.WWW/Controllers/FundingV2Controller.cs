using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.BusinessLogic.Funding;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Funding;
using HE.Investments.Loans.Contract.Funding.Commands;
using HE.Investments.Loans.Contract.Funding.Enums;
using HE.Investments.Loans.Contract.Funding.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

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
    public async Task<IActionResult> StartFunding(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.GetEmpty));
        if (response.ViewModel.IsReadOnly())
        {
            return RedirectToAction("CheckAnswers", new { Id = id });
        }

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
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.GDV));
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

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("GrossDevelopmentValue", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("estimated-total-costs")]
    [WorkflowState(FundingState.TotalCosts)]
    public async Task<IActionResult> EstimatedTotalCosts(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.EstimatedTotalCosts));
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

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("EstimatedTotalCosts", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("abnormal-costs")]
    [WorkflowState(FundingState.AbnormalCosts)]
    public async Task<IActionResult> AbnormalCosts(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.AbnormalCosts));
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

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("AbnormalCosts", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("private-sector-funding")]
    [WorkflowState(FundingState.PrivateSectorFunding)]
    public async Task<IActionResult> PrivateSectorFunding(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.PrivateSectorFunding));
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

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("PrivateSectorFunding", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("repayment-system")]
    [WorkflowState(FundingState.Refinance)]
    public async Task<IActionResult> RepaymentSystem(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.RepaymentSystem));
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

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("RepaymentSystem", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("additional-projects")]
    [WorkflowState(FundingState.AdditionalProjects)]
    public async Task<IActionResult> AdditionalProjects(Guid id)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.AdditionalProjects));
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

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("AdditionalProjects", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("check-answers")]
    [WorkflowState(FundingState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetFundingQuery(LoanApplicationId.From(id), FundingFieldsSet.GetAllFields), cancellationToken);
        return View("CheckAnswers", response.ViewModel);
    }

    [HttpPost("check-answers")]
    [WorkflowState(FundingState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswersPost(Guid id, FundingViewModel viewModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CheckAnswersFundingSectionCommand(LoanApplicationId.From(id), viewModel.CheckAnswers), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            viewModel.SetLoanApplicationId(id);

            return View("CheckAnswers", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(Guid id, FundingState currentPage)
    {
        return await Back(currentPage, new { Id = id });
    }

    protected override async Task<IStateRouting<FundingState>> Routing(FundingState currentState, object routeData = null)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;

        var applicationId = !string.IsNullOrEmpty(id) ? LoanApplicationId.From(Guid.Parse(id)) : null;
        var response = await _mediator.Send(new GetFundingQuery(applicationId!, FundingFieldsSet.GetAllFields));

        return new FundingWorkflow(currentState, response.ViewModel);
    }
}
