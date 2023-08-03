using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Common.Utils.ValueObjects;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.WWW.Models;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application")]
[Authorize]
public class LoanApplicationV2Controller : WorkflowController<LoanApplicationWorkflow.State>
{
    private readonly IMediator _mediator;
    private readonly IValidator<LoanPurposeModel> _validator;
    private readonly ICacheService _cacheService;

    public LoanApplicationV2Controller(IMediator mediator, IValidator<LoanPurposeModel> validator, ICacheService cacheService)
    {
        _mediator = mediator;
        _validator = validator;
        _cacheService = cacheService;
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
    public Task<IActionResult> AboutLoanPost()
    {
        return Continue();
    }

    [HttpGet("check-your-details")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckYourDetails)]
    public async Task<IActionResult> CheckYourDetails(CancellationToken cancellationToken)
    {
        var organizationBasicInformationResponse = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);
        var userDetails = await _mediator.Send(new GetUserDetailsQuery(), cancellationToken);
        return View(
            "CheckYourDetails",
            new CheckYourDetailsModel
            {
                OrganizationBasicInformation = organizationBasicInformationResponse.OrganizationBasicInformation,
                LoanApplicationContactEmail = userDetails.Email,
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
            var loanApplicationId = await _mediator.Send(new StartApplicationCommand(), cancellationToken);
            return RedirectToAction(nameof(TaskList), new { id = loanApplicationId.Value });
        }

        return RedirectToAction(nameof(Ineligible));
    }

    [HttpGet("ineligible")]
    [WorkflowState(LoanApplicationWorkflow.State.Ineligible)]
    public IActionResult Ineligible()
    {
        return View("Ineligible");
    }

    [HttpGet("{id}/task-list")]
    [WorkflowState(LoanApplicationWorkflow.State.TaskList)]
    public async Task<IActionResult> TaskList(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));

        var (isDeletedProjectInCache, deletedProjectFromCache) = response.LoanApplication.LegacyModel.ToggleDeleteProjectName(_cacheService);

        if (isDeletedProjectInCache)
        {
            ViewBag.AdditionalData = deletedProjectFromCache;
        }

        return View("TaskList", response.LoanApplication.LegacyModel);
    }

    [HttpPost("task-list/{id}")]
    [WorkflowState(LoanApplicationWorkflow.State.TaskList)]
    public Task<IActionResult> TaskListPost(Guid id)
    {
        return Continue(new { Id = id });
    }

    [HttpPost("task-list/{id}")]
    [WorkflowState(LoanApplicationWorkflow.State.TaskList)]
    public Task<IActionResult> TaskListPost(Guid id)
    {
        return Continue(new { Id = id });
    }

    [HttpGet("{id}/check")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckApplication)]
    public async Task<IActionResult> CheckApplication(Guid id)
    {
        var response = await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));

        return View("CheckApplication", response.LoanApplication.LegacyModel);
    }

    [HttpPost("{id}/submit")]
    [WorkflowState(LoanApplicationWorkflow.State.CheckApplication)]
    public async Task<IActionResult> Submit(Guid id)
    {
        await _mediator.Send(new SubmitLoanApplicationCommand(LoanApplicationId.From(id)));

        await _mediator.Send(new SubmitApplicationCommand(application.ViewModel));

        return await Continue(new { Id = id });
    }

    [HttpGet("{id}/submitted")]
    [WorkflowState(LoanApplicationWorkflow.State.ApplicationSubmitted)]
    public async Task<IActionResult> ApplicationSubmitted(Guid id)
    {
        var response = await _mediator.Send(new GetSubmitLoanApplicationQuery(LoanApplicationId.From(id)));

        return View("ApplicationSubmitted", response.LoanApplication);
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(LoanApplicationWorkflow.State currentPage, Guid applicationId)
    {
        return Back(currentPage, new { Id = applicationId });
    }

    protected override IStateRouting<LoanApplicationWorkflow.State> Routing(LoanApplicationWorkflow.State currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;

        var applicationId = !string.IsNullOrEmpty(id) ? LoanApplicationId.From(Guid.Parse(id)) : null;

        return new LoanApplicationWorkflow(applicationId, _mediator, currentState);
    }
}
