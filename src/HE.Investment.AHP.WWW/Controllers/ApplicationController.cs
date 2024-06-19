using System.Globalization;
using HE.Investment.AHP.Contract.AhpProgramme;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.Application.Factories;
using HE.Investment.AHP.WWW.Models.Project;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.Authorization;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize(ConsortiumAccessContext.Edit)]
[Route("{organisationId}/application")]
public class ApplicationController : WorkflowController<ApplicationWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly IApplicationSummaryViewModelFactory _applicationSummaryViewModelFactory;

    public ApplicationController(
        IMediator mediator,
        IApplicationSummaryViewModelFactory applicationSummaryViewModelFactory)
    {
        _mediator = mediator;
        _applicationSummaryViewModelFactory = applicationSummaryViewModelFactory;
    }

    [ConsortiumAuthorize]
    [HttpGet]
    [WorkflowState(ApplicationWorkflowState.ApplicationsList)]
    public IActionResult Index(string? projectId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            return this.OrganisationRedirectToAction("Index", "Projects");
        }

        return this.OrganisationRedirectToAction("Applications", "Project", new { projectId });
    }

    [HttpGet("start")]
    [WorkflowState(ApplicationWorkflowState.Start)]
    public async Task<IActionResult> Start([FromQuery] string projectId, CancellationToken cancellationToken)
    {
        var ahpProgramme = await _mediator.Send(new GetTheAhpProgrammeQuery(), cancellationToken);
        return View("Splash", new ProjectBasicModel(projectId, ahpProgramme));
    }

    [HttpPost("start")]
    [WorkflowState(ApplicationWorkflowState.Start)]
    public IActionResult StartPost([FromQuery] string projectId)
    {
        return this.OrganisationRedirectToAction("Select", "Site", new { projectId });
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpGet("/{organisationId}/site/{siteId}/application/name")]
    public async Task<IActionResult> Name([FromRoute] string siteId, [FromQuery] string? applicationName, CancellationToken cancellationToken)
    {
        var site = await GetSiteDetails(siteId, cancellationToken);
        if (site.Status == SiteStatus.Submitted)
        {
            return View("Name", new ApplicationBasicModel(site.ProjectId, null, applicationName, Contract.Application.Tenure.Undefined));
        }

        return this.OrganisationRedirectToAction("Start", "Site", new { siteId = site.Id });
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpPost("/{organisationId}/site/{siteId}/application/name")]
    public async Task<IActionResult> Name([FromRoute] string siteId, ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new IsApplicationNameAvailableQuery(model.Name), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await this.ReturnToProjectOrContinue(
            async () => await Continue(new { applicationName = model.Name, siteId, projectId = model.ProjectId }),
            model.ProjectId);
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpGet("/{organisationId}/site/{siteId}/application/tenure")]
    public IActionResult Tenure([FromQuery] string applicationName, [FromQuery] string projectId)
    {
        return View("Tenure", new ApplicationBasicModel(projectId, null, applicationName, Contract.Application.Tenure.Undefined));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpPost("/{organisationId}/site/{siteId}/application/tenure")]
    public async Task<IActionResult> Tenure([FromRoute] string siteId, ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateApplicationCommand(SiteId.From(siteId), model.Name, model.Tenure), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await this.ReturnToProjectOrContinue(
            async () => await Task.FromResult(this.OrganisationRedirectToAction(nameof(TaskList), routeValues: new { applicationId = result.ReturnedData.Value })),
            model.ProjectId);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.TaskList)]
    [HttpGet("{applicationId}/task-list")]
    [HttpGet("{applicationId}")]
    public async Task<IActionResult> TaskList(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        var site = await _mediator.Send(new GetSiteBasicDetailsQuery(application.SiteId.Value), cancellationToken);

        var model = new ApplicationSectionsModel(
            applicationId,
            application.ProjectId.Value,
            site.Name,
            application.Name,
            application.Status,
            application.AllowedOperations,
            application.ReferenceNumber,
            application.LastModificationDetails,
            application.LastSubmissionDetails,
            application.Sections);

        return View("TaskList", model);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.CheckAnswers)]
    [HttpGet("{applicationId}/check-answers")]
    public async Task<IActionResult> CheckAnswers(string applicationId, CancellationToken cancellationToken)
    {
        var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(
            AhpApplicationId.From(applicationId),
            Url,
            cancellationToken);

        return View("CheckAnswers", applicationSummary);
    }

    [ConsortiumAuthorize(ConsortiumAccessContext.Submit)]
    [WorkflowState(ApplicationWorkflowState.CheckAnswers)]
    [HttpPost("{applicationId}/check-answers")]
    public async Task<IActionResult> CheckAnswersPost(string applicationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CheckAnswersCommand(AhpApplicationId.From(applicationId)), cancellationToken);

        if (result.HasValidationErrors)
        {
            var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(AhpApplicationId.From(applicationId), Url, cancellationToken);

            ModelState.AddValidationErrors(result);
            return View("CheckAnswers", applicationSummary);
        }

        return await Continue(new { applicationId });
    }

    [WorkflowState(ApplicationWorkflowState.Submit)]
    [HttpGet("{applicationId}/submit")]
    public async Task<IActionResult> Submit(string applicationId, CancellationToken cancellationToken)
    {
        var model = await GetApplicationSubmitModel(applicationId, cancellationToken);

        return View(nameof(Submit), model);
    }

    [ConsortiumAuthorize(ConsortiumAccessContext.Submit)]
    [WorkflowState(ApplicationWorkflowState.Submit)]
    [HttpPost("{applicationId}/submit")]
    public async Task<IActionResult> Submit(string applicationId, ApplicationSubmitModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ApplicationSubmitModel>(
            _mediator,
            new SubmitApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.RepresentationsAndWarranties),
            () => ContinueWithRedirect(new { model.ApplicationId }),
            async () =>
            {
                model = await GetApplicationSubmitModel(applicationId, cancellationToken);

                return View(nameof(Submit), model);
            },
            cancellationToken);
    }

    [ConsortiumAuthorize(ConsortiumAccessContext.Submit)]
    [WorkflowState(ApplicationWorkflowState.Completed)]
    [HttpGet("{applicationId}/completed")]
    public async Task<IActionResult> Completed(string applicationId, CancellationToken cancellationToken)
    {
        var model = await GetApplicationSubmitModel(applicationId, cancellationToken);

        return View(nameof(Completed), model);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.OnHold)]
    [HttpGet("{applicationId}/on-hold")]
    public async Task<IActionResult> OnHold(string applicationId, CancellationToken cancellationToken)
    {
        return await ReturnViewToChangeApplicationStatus(applicationId, cancellationToken);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.OnHold)]
    [HttpPost("{applicationId}/on-hold")]
    public async Task<IActionResult> OnHold(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new HoldApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.HoldReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("OnHold", model)),
            cancellationToken);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.Reactivate)]
    [HttpGet("{applicationId}/reactivate")]
    public async Task<IActionResult> Reactivate(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new ReactivateApplicationCommand(AhpApplicationId.From(model.ApplicationId)),
            () => Continue(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("TaskList")),
            cancellationToken);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.RequestToEdit)]
    [HttpGet("{applicationId}/request-to-edit")]
    public async Task<IActionResult> RequestToEdit(string applicationId, CancellationToken cancellationToken)
    {
        return await ReturnViewToChangeApplicationStatus(applicationId, cancellationToken);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.RequestToEdit)]
    [HttpPost("{applicationId}/request-to-edit")]
    public async Task<IActionResult> RequestToEdit(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new RequestToEditApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.RequestToEditReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("RequestToEdit", model)),
            cancellationToken);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.Withdraw)]
    [HttpGet("{applicationId}/withdraw")]
    public async Task<IActionResult> Withdraw(string applicationId, CancellationToken cancellationToken)
    {
        return await ReturnViewToChangeApplicationStatus(applicationId, cancellationToken);
    }

    [ConsortiumAuthorize]
    [WorkflowState(ApplicationWorkflowState.Withdraw)]
    [HttpPost("{applicationId}/withdraw")]
    public async Task<IActionResult> Withdraw(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new WithdrawApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.WithdrawReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("Withdraw", model)),
            cancellationToken);
    }

    [ConsortiumAuthorize]
    [HttpGet("{applicationId}/back")]
    public async Task<IActionResult> Back([FromRoute] string applicationId, string? projectId, ApplicationWorkflowState currentPage)
    {
        return await Back(currentPage, new { applicationId, projectId });
    }

    protected override Task<IStateRouting<ApplicationWorkflowState>> Routing(ApplicationWorkflowState currentState, object? routeData = null)
    {
        var applicationId = this.TryGetApplicationIdFromRoute();

        return Task.FromResult<IStateRouting<ApplicationWorkflowState>>(new ApplicationWorkflow(
            currentState,
            async () => await _mediator.Send(new GetApplicationQuery(applicationId!)),
            async () => applicationId.IsProvided() && await _mediator.Send(new IsApplicationExistQuery(applicationId!))));
    }

    private async Task<IActionResult> ReturnViewToChangeApplicationStatus(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        var model = new ChangeApplicationStatusModel(
            applicationId,
            application.Name);

        return View(model);
    }

    private async Task<ApplicationSubmitModel> GetApplicationSubmitModel(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationDetailsQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        var siteBasicModel = await _mediator.Send(new GetSiteBasicDetailsQuery(application.SiteId.Value), cancellationToken);

        return new ApplicationSubmitModel(
            application.ApplicationId.Value,
            application.ProjectId.Value,
            application.ApplicationName,
            application.ReferenceNumber,
            siteBasicModel.Name,
            application.Tenure,
            application.NumberOfHomes?.ToString(CultureInfo.InvariantCulture)!,
            application.FundingRequested.DisplayPounds()!,
            application.TotalSchemeCost.DisplayPounds()!,
            application.RepresentationsAndWarranties == true ? "checked" : null);
    }

    private async Task<SiteModel> GetSiteDetails(string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        ViewBag.ProjectId = siteModel.ProjectId;
        return siteModel;
    }
}
