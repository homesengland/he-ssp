using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Domain.Application.Commands;
using HE.Investment.AHP.Domain.Application.Workflows;
using HE.Investment.AHP.WWW.Models.Application;
using HE.InvestmentLoans.Common.Routing;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Validators;
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

    public ApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var applications = await _mediator.Send(new GetApplicationsQuery(), cancellationToken);

        return View("Index", new ApplicationsModel(_siteName, applications.Select(CreateModel).ToList()));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpGet("name")]
    [HttpGet("{applicationId}/name")]
    public async Task<IActionResult> Name(string? applicationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(applicationId))
        {
            return View();
        }

        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

        return View("Name", CreateModel(application));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpPost("name")]
    [HttpPost("{applicationId}/name")]
    public async Task<IActionResult> Name(ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = string.IsNullOrWhiteSpace(model.Id)
            ? await _mediator.Send(new CreateApplicationCommand(model.Name), cancellationToken)
            : await _mediator.Send(new UpdateApplicationNameCommand(model.Id, model.Name), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationId = result.ReturnedData!.Value });
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpGet("{applicationId}/tenure")]
    public async Task<IActionResult> Tenure(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

        return View("Tenure", CreateModel(application));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpPost("{applicationId}/tenure")]
    public async Task<IActionResult> Tenure(string applicationId, ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateApplicationTenureCommand(applicationId, model.Tenure), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return RedirectToAction("TaskList", new { applicationId = model.Id });
    }

    [HttpGet("{applicationId}/task-list")]
    public async Task<IActionResult> TaskList(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

        var model = new ApplicationModel(_siteName, application.Name, application.Sections);

        return View("TaskList", model);
    }

    protected override async Task<IStateRouting<ApplicationWorkflowState>> Routing(ApplicationWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult(new ApplicationWorkflow(currentState));
    }

    private static ApplicationBasicModel CreateModel(Application application)
    {
        return new ApplicationBasicModel(application.Id, application.Name, application.Tenure);
    }
}
