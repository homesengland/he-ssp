using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Security;
using HE.InvestmentLoans.Contract.Security.Commands;
using HE.InvestmentLoans.Contract.Security.Queries;
using HE.InvestmentLoans.Contract.Security.ValueObjects;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[SuppressMessage("Usage", "CA1801", Justification = "It should be fixed in the future")]
[Route("application/{id}/security")]
[Authorize]
public class SecurityV2Controller : WorkflowController<SecurityState>
{
    private readonly IMediator _mediator;
    private readonly IValidator<SecurityViewModel> _validator;

    public SecurityV2Controller(IMediator mediator, IValidator<SecurityViewModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("")]
    [WorkflowState(SecurityState.Index)]
    public IActionResult StartSecurity(Guid id)
    {
        return View("Index", LoanApplicationId.From(id));
    }

    [HttpPost("start")]
    [WorkflowState(SecurityState.Index)]
    public Task<IActionResult> StartSecurityPost(Guid id)
    {
        return Continue(new { Id = id });
    }

    [HttpGet("charges-debt")]
    [WorkflowState(SecurityState.ChargesDebtCompany)]
    public async Task<IActionResult> ChargesDebtCompany(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);

        return View("ChargesDebtCompany", response.ViewModel);
    }

    [HttpPost("charges-debt")]
    [WorkflowState(SecurityState.ChargesDebtCompany)]
    public async Task<IActionResult> ChargesDebtCompanyPost(Guid id, SecurityViewModel viewModel, [FromQuery] string redirect, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideCompanyDebenture(LoanApplicationId.From(id), viewModel.ChargesDebtCompany, viewModel.ChargesDebtCompanyInfo), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);
            return View("ChargesDebtCompany", response.ViewModel);
        }

        return redirect.IsProvided() ?
            Change(redirect, new { Id = id }) :
            await Continue(new { Id = id });
    }

    [HttpGet("dir-loans")]
    [WorkflowState(SecurityState.DirLoans)]
    public async Task<IActionResult> DirLoans(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);

        return View("DirLoans", response.ViewModel);
    }

    [HttpPost("dir-loans")]
    [WorkflowState(SecurityState.DirLoans)]
    public async Task<IActionResult> DirLoansPost(Guid id, SecurityViewModel viewModel, [FromQuery] string redirect, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideDirectorLoansCommand(LoanApplicationId.From(id), viewModel.DirLoans), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);
            return View("DirLoans", response.ViewModel);
        }

        return redirect.IsProvided() ?
            Change(redirect, new { Id = id }) :
            await Continue(new { Id = id });
    }

    [HttpGet("dir-loans-sub")]
    [WorkflowState(SecurityState.DirLoansSub)]
    public async Task<IActionResult> DirLoansSub(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);

        return View("DirLoansSub", response.ViewModel);
    }

    [HttpPost("dir-loans-sub")]
    [WorkflowState(SecurityState.DirLoansSub)]
    public async Task<IActionResult> DirLoansSubPost(Guid id, SecurityViewModel viewModel, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideDirectorLoansSubordinateCommand(LoanApplicationId.From(id), viewModel.DirLoansSub, viewModel.DirLoansSubMore), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);
            return View("DirLoansSub", response.ViewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("check-answers")]
    [WorkflowState(SecurityState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);

        return View("CheckAnswers", response.ViewModel);
    }

    [HttpPost("check-answers")]
    [WorkflowState(SecurityState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswersPost(Guid id, SecurityViewModel viewModel, CancellationToken token)
    {
        var result = await _mediator.Send(new ConfirmSecuritySectionCommand(LoanApplicationId.From(id), viewModel.CheckAnswers), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id)), token);
            return View("CheckAnswers", response.ViewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(SecurityState currentPage, Guid id)
    {
        return Back(currentPage, new { Id = id });
    }

    protected override IStateRouting<SecurityState> Routing(SecurityState currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;

        var applicationId = !string.IsNullOrEmpty(id) ? LoanApplicationId.From(Guid.Parse(id)) : null;

        var response = _mediator.Send(new GetSecurity(LoanApplicationId.From(id))).Result;

        return new SecurityWorkflow(applicationId, response.ViewModel, _mediator, currentState);
    }
}
