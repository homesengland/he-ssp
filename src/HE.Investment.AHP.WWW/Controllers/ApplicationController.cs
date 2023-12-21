using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Workflows;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.Application.Factories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Utils.Pagination;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Routing;
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

    [HttpGet("{applicationId}/task-list")]
    [HttpGet("{applicationId}")]
    public async Task<IActionResult> TaskList(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

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
        var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(applicationId, Url, isReadOnly, cancellationToken);

        return View("CheckAnswers", applicationSummary);
    }

    [HttpPost("{applicationId}/submit")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> Submit(string applicationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SubmitApplicationCommand(applicationId), cancellationToken);

        if (result.HasValidationErrors)
        {
            var isReadOnly = !await _accountAccessContext.CanEditApplication();
            var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(applicationId, Url, isReadOnly, cancellationToken);

            ModelState.AddValidationErrors(result);
            return View("CheckAnswers", applicationSummary);
        }

        return RedirectToAction(nameof(Submitted), new { applicationId });
    }

    [HttpGet("{applicationId}/submitted")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> Submitted(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

        // TODO: set job role and contact details
        return View(
            "Submitted",
            new ApplicationSubmittedViewModel(applicationId, application.ReferenceNumber ?? string.Empty, "[job role]", "[INSERT CONTACT DETAILS]"));
    }

    protected override async Task<IStateRouting<ApplicationWorkflowState>> Routing(ApplicationWorkflowState currentState, object? routeData = null)
    {
        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        return new ApplicationWorkflow(currentState, isReadOnly);
    }
}
