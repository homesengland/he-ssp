using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure;
using HE.InvestmentLoans.Contract.Funding.Enums;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Security;
using HE.InvestmentLoans.Contract.User.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Models;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application")]
[AuthorizeWithCompletedProfile]
public class LoanApplicationV2Controller : WorkflowController<LoanApplicationWorkflow.State>
{
    private readonly IMediator _mediator;
    private readonly IValidator<LoanPurposeModel> _validator;

    public LoanApplicationV2Controller(IMediator mediator, IValidator<LoanPurposeModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("")]
    [WorkflowState(LoanApplicationWorkflow.State.Index)]
    public IActionResult StartApplication()
    {
        return View("StartApplication");
    }

    [HttpPost("start-now")]
    [WorkflowState(LoanApplicationWorkflow.State.Index)]
    public async Task<IActionResult> StartNow()
    {
        return await Continue();
    }

    [HttpGet("about-loan")]
    [WorkflowState(LoanApplicationWorkflow.State.AboutLoan)]
    public IActionResult AboutLoan()
    {
        return View("AboutLoan");
    }

    [HttpPost("about-loan")]
    [WorkflowState(LoanApplicationWorkflow.State.AboutLoan)]
    public async Task<IActionResult> AboutLoanPost()
    {
        return await Continue();
    }

    [HttpGet("check-your-details")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckYourDetails)]
    public async Task<IActionResult> CheckYourDetails(CancellationToken cancellationToken)
    {
        var organizationBasicInformationResponse = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);
        var userDetails = await _mediator.Send(new GetUserAccountQuery(), cancellationToken);
        return View(
            "CheckYourDetails",
            new CheckYourDetailsModel
            {
                OrganizationBasicInformation = organizationBasicInformationResponse.OrganizationBasicInformation,
                LoanApplicationContactEmail = userDetails.Email,
                LoanApplicationContactName = $"{userDetails.FirstName} {userDetails.LastName}",
                LoanApplicationContactTelephoneNumber = userDetails.TelephoneNumber,
            });
    }

    [HttpPost("check-your-details")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckYourDetails)]
    public Task<IActionResult> CheckYourDetailsPost()
    {
        return Continue();
    }

    [HttpGet("loan-purpose")]
    [WorkflowState(LoanApplicationWorkflow.State.LoanPurpose)]
    public IActionResult LoanPurpose()
    {
        return View("LoanPurpose", new LoanPurposeModel());
    }

    [HttpPost("loan-purpose")]
    [WorkflowState(LoanApplicationWorkflow.State.LoanPurpose)]
    public async Task<IActionResult> LoanPurposePost(LoanPurposeModel model, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            return View("LoanPurpose", model);
        }

        if (model.FundingPurpose == FundingPurpose.BuildingNewHomes)
        {
            return RedirectToAction(nameof(ApplicationName));
        }

        return RedirectToAction(nameof(Ineligible));
    }

    [HttpGet("application-name")]
    [WorkflowState(LoanApplicationWorkflow.State.ApplicationName)]
    public IActionResult ApplicationName()
    {
        return View("ApplicationName", new ApplicationNameModel());
    }

    [HttpPost("application-name")]
    [WorkflowState(LoanApplicationWorkflow.State.ApplicationName)]
    public async Task<IActionResult> ApplicationNamePost(ApplicationNameModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new StartApplicationCommand(model.LoanApplicationName), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("ApplicationName", model);
        }

        return RedirectToAction(nameof(TaskList), new { id = result.ReturnedData!.Value });
    }

    [HttpGet("ineligible")]
    [WorkflowState(LoanApplicationWorkflow.State.Ineligible)]
    public IActionResult Ineligible()
    {
        return View("Ineligible");
    }

    [HttpGet("{id}/task-list")]
    [WorkflowState(LoanApplicationWorkflow.State.TaskList)]
    [WorkflowState(CompanyStructureState.Complete)]
    [WorkflowState(SecurityState.Complete)]
    [WorkflowState(FundingState.Complete)]
    [WorkflowState(ProjectState.Complete)]
    public async Task<IActionResult> TaskList(Guid id)
    {
        var response = await _mediator.Send(new GetTaskListDataQuery(LoanApplicationId.From(id)));

        return View("TaskListV2", response);
    }

    [HttpPost("{id}/task-list")]
    [WorkflowState(LoanApplicationWorkflow.State.TaskList)]
    public async Task<IActionResult> TaskListPost(Guid id)
    {
        return await Continue(new { Id = id });
    }

    [HttpGet("{id}/check")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckApplication)]
    public async Task<IActionResult> CheckApplication(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));

        return View("CheckApplication", response.LoanApplication);
    }

    [HttpPost("{id}/submit")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckApplication)]
    public async Task<IActionResult> Submit(Guid id)
    {
        await _mediator.Send(new SubmitLoanApplicationCommand(LoanApplicationId.From(id)));

        return await Continue(new { Id = id });
    }

    [HttpPost("{id}/resubmit")]
    [WorkflowState(LoanApplicationWorkflow.State.ResubmitApplication)]
    public async Task<IActionResult> Resubmit(Guid id)
    {
        await _mediator.Send(new SubmitLoanApplicationCommand(LoanApplicationId.From(id)));

        return await Continue(new { Id = id });
    }

    [HttpGet("{id}/submitted")]
    [WorkflowState(LoanApplicationWorkflow.State.ApplicationSubmitted)]
    public async Task<IActionResult> ApplicationSubmitted(Guid id)
    {
        var response = await _mediator.Send(new GetSubmitLoanApplicationQuery(LoanApplicationId.From(id)));

        return View("ApplicationSubmitted", response.LoanApplication);
    }

    [HttpGet("{id}/dashboard")]
    [WorkflowState(LoanApplicationWorkflow.State.ApplicationDashboard)]
    public async Task<IActionResult> ApplicationDashboard(Guid id)
    {
        var response = await _mediator.Send(new GetApplicationDashboardQuery(LoanApplicationId.From(id)));

        return View(
            "ApplicationDashboard",
            new ApplicationDashboardModel
            {
                LoanApplicationId = LoanApplicationId.From(id),
                LoanApplicationStatus = response.ApplicationStatus,
                Data = response,
                IsOverviewSectionSelected = true,
            });
    }

    [HttpGet("{id}/dashboard/supporting-documents")]
    [WorkflowState(LoanApplicationWorkflow.State.ApplicationDashboard)]
    public async Task<IActionResult> ApplicationDashboardSupportingDocuments(Guid id)
    {
        var response = await _mediator.Send(new GetApplicationDashboardQuery(LoanApplicationId.From(id)));

        return View("ApplicationDashboard", new ApplicationDashboardModel { LoanApplicationId = LoanApplicationId.From(id), Data = response, IsOverviewSectionSelected = false });
    }

    [HttpGet("{id}/withdraw")]
    [WorkflowState(LoanApplicationWorkflow.State.Withdraw)]
    public IActionResult Withdraw(Guid id)
    {
        return View("Withdraw", new WithdrawModel { LoanApplicationId = LoanApplicationId.From(id) });
    }

    [HttpPost("{id}/withdraw")]
    [WorkflowState(LoanApplicationWorkflow.State.Withdraw)]
    public async Task<IActionResult> WithdrawPost(Guid id, WithdrawModel withdrawModel, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new WithdrawLoanApplicationCommand(LoanApplicationId.From(id), withdrawModel.WithdrawReason), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Withdraw", new WithdrawModel { LoanApplicationId = LoanApplicationId.From(id) });
        }

        return await Continue(new { Id = id });
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(LoanApplicationWorkflow.State currentPage, Guid applicationId)
    {
        return Back(currentPage, new { Id = applicationId });
    }

    protected override Task<IStateRouting<LoanApplicationWorkflow.State>> Routing(LoanApplicationWorkflow.State currentState, object routeData = null)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;

        var applicationId = !string.IsNullOrEmpty(id) ? LoanApplicationId.From(Guid.Parse(id)) : null;

        return Task.FromResult<IStateRouting<LoanApplicationWorkflow.State>>(
            new LoanApplicationWorkflow(
                currentState,
                async () => (await _mediator.Send(new GetLoanApplicationQuery(applicationId!))).LoanApplication,
                async () => applicationId.IsProvided() && await _mediator.Send(new IsLoanApplicationExistQuery(applicationId!))));
    }
}
