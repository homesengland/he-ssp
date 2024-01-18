using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.Application.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("application")]
[AuthorizeWithCompletedProfile]
public class ApplicationController : WorkflowController<ApplicationWorkflowState>
{
    private readonly string _siteName = "Test Site";
    private readonly IMediator _mediator;
    private readonly IApplicationSummaryViewModelFactory _applicationSummaryViewModelFactory;
    private readonly IAccountAccessContext _accountAccessContext;

    public ApplicationController(
        IMediator mediator,
        IApplicationSummaryViewModelFactory applicationSummaryViewModelFactory,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _applicationSummaryViewModelFactory = applicationSummaryViewModelFactory;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? page, CancellationToken cancellationToken)
    {
        var applicationsQueryResult = await _mediator.Send(new GetApplicationsQuery(new PaginationRequest(page ?? 1)), cancellationToken);
        var isReadOnly = !await _accountAccessContext.CanEditApplication();

        return View("Index", new ApplicationsListModel(applicationsQueryResult.OrganisationName, applicationsQueryResult.PaginationResult, isReadOnly));
    }

    [HttpGet("start")]
    [WorkflowState(ApplicationWorkflowState.Start)]
    public async Task<IActionResult> Start(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteListQuery(), cancellationToken);
        return View("Splash", response);
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpGet("name")]
    public IActionResult Name([FromQuery] string? applicationName)
    {
        return View("Name", new ApplicationBasicModel(null, applicationName, Contract.Application.Tenure.Undefined));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpPost("name")]
    public async Task<IActionResult> Name(ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new IsApplicationNameAvailableQuery(model.Name), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationName = model.Name });
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpGet("tenure")]
    public IActionResult Tenure([FromQuery] string applicationName)
    {
        return View("Tenure", new ApplicationBasicModel(null, applicationName, Contract.Application.Tenure.Undefined));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpPost("tenure")]
    public async Task<IActionResult> Tenure(ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateApplicationCommand(model.Name, model.Tenure), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return RedirectToAction(nameof(TaskList), new { applicationId = result.ReturnedData.Value });
    }

    [WorkflowState(ApplicationWorkflowState.TaskList)]
    [HttpGet("{applicationId}/task-list")]
    [HttpGet("{applicationId}")]
    public async Task<IActionResult> TaskList(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        var model = new ApplicationSectionsModel(
            applicationId,
            _siteName,
            application.Name,
            application.Status,
            application.ReferenceNumber,
            application.LastModificationDetails,
            application.Sections);

        return View("TaskList", model);
    }

    [HttpGet("{applicationId}/check-answers")]
    public async Task<IActionResult> CheckAnswers(string applicationId, CancellationToken cancellationToken)
    {
        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(AhpApplicationId.From(applicationId), Url, isReadOnly, cancellationToken);

        return View("CheckAnswers", applicationSummary);
    }

    [HttpPost("{applicationId}/submit")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> Submit(string applicationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SubmitApplicationCommand(AhpApplicationId.From(applicationId)), cancellationToken);

        if (result.HasValidationErrors)
        {
            var isReadOnly = !await _accountAccessContext.CanEditApplication();
            var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(AhpApplicationId.From(applicationId), Url, isReadOnly, cancellationToken);

            ModelState.AddValidationErrors(result);
            return View("CheckAnswers", applicationSummary);
        }

        return RedirectToAction(nameof(Submitted), new { applicationId });
    }

    [HttpGet("{applicationId}/submitted")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> Submitted(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        // TODO: set job role and contact details
        return View(
            "Submitted",
            new ApplicationSubmittedViewModel(applicationId, application.ReferenceNumber ?? string.Empty, "[job role]", "[INSERT CONTACT DETAILS]"));
    }

    [WorkflowState(ApplicationWorkflowState.OnHold)]
    [HttpGet("{applicationId}/on-hold")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> OnHold(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        var model = new ChangeApplicationStatusModel(
            applicationId,
            application.Name);

        return View(model);
    }

    [WorkflowState(ApplicationWorkflowState.OnHold)]
    [HttpPost("{applicationId}/on-hold")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> OnHold(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand(
            _mediator,
            new HoldApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.ChangeStatusReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("OnHold", model)),
            cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.Withdraw)]
    [HttpGet("{applicationId}/withdraw")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Withdraw(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        var model = new ChangeApplicationStatusModel(
            applicationId,
            application.Name);

        return View(model);
    }

    [WorkflowState(ApplicationWorkflowState.Withdraw)]
    [HttpPost("{applicationId}/withdraw")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Withdraw(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand(
            _mediator,
            new WithdrawApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.ChangeStatusReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("Withdraw", model)),
            cancellationToken);
    }

    protected override async Task<IStateRouting<ApplicationWorkflowState>> Routing(ApplicationWorkflowState currentState, object? routeData = null)
    {
        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        return new ApplicationWorkflow(currentState, isReadOnly);
    }
}
