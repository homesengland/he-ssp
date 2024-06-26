using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using HE.Investments.Loans.Contract.Security;
using HE.Investments.Loans.Contract.Security.Commands;
using HE.Investments.Loans.Contract.Security.Queries;
using HE.Investments.Loans.WWW.Workflows;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

[Route("{organisationId}/application/{id}/security")]
[AuthorizeWithCompletedProfile]
public class SecurityController : WorkflowController<SecurityState>
{
    private readonly IMediator _mediator;

    public SecurityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [WorkflowState(SecurityState.Index)]
    public async Task<IActionResult> StartSecurity(Guid id)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id), SecurityFieldsSet.GetEmpty));
        if (response.ViewModel.IsReadOnly())
        {
            return this.OrganisationRedirectToAction("CheckAnswers", routeValues: new { Id = id });
        }

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
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id), SecurityFieldsSet.ChargesDebtCompany), token);

        return View("ChargesDebtCompany", response.ViewModel);
    }

    [HttpPost("charges-debt")]
    [WorkflowState(SecurityState.ChargesDebtCompany)]
    public async Task<IActionResult> ChargesDebtCompanyPost(Guid id, SecurityViewModel viewModel, [FromQuery] string redirect, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideCompanyDebenture(
                LoanApplicationId.From(id),
                viewModel.ChargesDebtCompany ?? string.Empty,
                viewModel.ChargesDebtCompanyInfo ?? string.Empty),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("ChargesDebtCompany", viewModel);
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("dir-loans")]
    [WorkflowState(SecurityState.DirLoans)]
    public async Task<IActionResult> DirLoans(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id), SecurityFieldsSet.DirLoans), token);

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
            return View("DirLoans", viewModel);
        }

        if (viewModel.DirLoans == CommonResponse.Yes)
        {
            return await Continue(new { id, redirect });
        }

        return await Continue(redirect, new { Id = id });
    }

    [HttpGet("dir-loans-sub")]
    [WorkflowState(SecurityState.DirLoansSub)]
    public async Task<IActionResult> DirLoansSub(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id), SecurityFieldsSet.DirLoansSub), token);

        return View("DirLoansSub", response.ViewModel);
    }

    [HttpPost("dir-loans-sub")]
    [WorkflowState(SecurityState.DirLoansSub)]
    public async Task<IActionResult> DirLoansSubPost(Guid id, SecurityViewModel viewModel, CancellationToken token)
    {
        var result = await _mediator.Send(
            new ProvideDirectorLoansSubordinateCommand(
                LoanApplicationId.From(id),
                viewModel.DirLoansSub,
                viewModel.DirLoansSubMore),
            token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("DirLoansSub", viewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("check-answers")]
    [WorkflowState(SecurityState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, CancellationToken token)
    {
        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id), SecurityFieldsSet.GetAllFields), token);

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

            var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id), SecurityFieldsSet.GetAllFields), token);
            return View("CheckAnswers", response.ViewModel);
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(SecurityState currentPage, Guid id)
    {
        return Back(currentPage, new { Id = id });
    }

    protected override async Task<IStateRouting<SecurityState>> Routing(SecurityState currentState, object routeData = null)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;

        var response = await _mediator.Send(new GetSecurity(LoanApplicationId.From(id!), SecurityFieldsSet.GetAllFields));

        return new SecurityWorkflow(currentState, response.ViewModel);
    }
}
