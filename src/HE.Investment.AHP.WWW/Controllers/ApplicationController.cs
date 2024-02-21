using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Commands;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.Application.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Helpers;
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
    [WorkflowState(ApplicationWorkflowState.ApplicationsList)]
    public async Task<IActionResult> Index([FromQuery] int? page, CancellationToken cancellationToken)
    {
        var applicationsQueryResult = await _mediator.Send(new GetApplicationsQuery(new PaginationRequest(page ?? 1)), cancellationToken);
        var isReadOnly = !await _accountAccessContext.CanEditApplication();

        return View("Index", new ApplicationsListModel(applicationsQueryResult.OrganisationName, applicationsQueryResult.PaginationResult, isReadOnly));
    }

    [HttpGet("start")]
    [WorkflowState(ApplicationWorkflowState.Start)]
    public IActionResult Start()
    {
        return View("Splash");
    }

    [HttpPost("start")]
    [WorkflowState(ApplicationWorkflowState.Start)]
    public async Task<IActionResult> StartPost(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(1, 1)), cancellationToken);
        if (response.Page.Items.Any())
        {
            return RedirectToAction("Select", "Site");
        }

        return RedirectToAction("Start", "Site");
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpGet("/{siteId}/application/name")]
    public IActionResult Name([FromQuery] string? applicationName)
    {
        return View("Name", new ApplicationBasicModel(null, applicationName, Contract.Application.Tenure.Undefined));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationName)]
    [HttpPost("/{siteId}/application/name")]
    public async Task<IActionResult> Name([FromRoute] string siteId, ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new IsApplicationNameAvailableQuery(model.Name), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationName = model.Name, siteId });
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpGet("/{siteId}/application/tenure")]
    public IActionResult Tenure([FromQuery] string applicationName)
    {
        return View("Tenure", new ApplicationBasicModel(null, applicationName, Contract.Application.Tenure.Undefined));
    }

    [WorkflowState(ApplicationWorkflowState.ApplicationTenure)]
    [HttpPost("/{siteId}/application/tenure")]
    public async Task<IActionResult> Tenure([FromRoute] string siteId, ApplicationBasicModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateApplicationCommand(new SiteId(siteId), model.Name, model.Tenure), cancellationToken);

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

    [WorkflowState(ApplicationWorkflowState.CheckAnswers)]
    [HttpGet("{applicationId}/check-answers")]
    public async Task<IActionResult> CheckAnswers(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        var isReadOnly = !await _accountAccessContext.CanEditApplication() || application.IsReadOnly;
        var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(AhpApplicationId.From(applicationId), Url, isReadOnly, cancellationToken);

        return View("CheckAnswers", applicationSummary);
    }

    [WorkflowState(ApplicationWorkflowState.CheckAnswers)]
    [HttpPost("{applicationId}/check-answers")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> CheckAnswersPost(string applicationId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CheckAnswersCommand(AhpApplicationId.From(applicationId)), cancellationToken);

        if (result.HasValidationErrors)
        {
            var isReadOnly = !await _accountAccessContext.CanEditApplication();
            var applicationSummary = await _applicationSummaryViewModelFactory.GetDataAndCreate(AhpApplicationId.From(applicationId), Url, isReadOnly, cancellationToken);

            ModelState.AddValidationErrors(result);
            return View("CheckAnswers", applicationSummary);
        }

        return await Continue(new { applicationId });
    }

    [WorkflowState(ApplicationWorkflowState.Submit)]
    [HttpGet("{applicationId}/submit")]
    public async Task<IActionResult> Submit(string applicationId, CancellationToken cancellationToken)
    {
        var model = await GetAplicationSubmitModel(applicationId, cancellationToken);

        return View(nameof(Submit), model);
    }

    [WorkflowState(ApplicationWorkflowState.Submit)]
    [HttpPost("{applicationId}/submit")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> Submit(string applicationId, ApplicationSubmitModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ApplicationSubmitModel>(
            _mediator,
            new SubmitApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.RepresentationsAndWarranties),
            () => ContinueWithRedirect(new { model.ApplicationId }),
            async () =>
            {
                model = await GetAplicationSubmitModel(applicationId, cancellationToken);

                return View(nameof(Submit), model);
            },
            cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.Completed)]
    [HttpGet("{applicationId}/completed")]
    [AuthorizeWithCompletedProfile(AccountAccessContext.SubmitApplication)]
    public async Task<IActionResult> Completed(string applicationId, CancellationToken cancellationToken)
    {
        var model = await GetAplicationSubmitModel(applicationId, cancellationToken);

        return View(nameof(Completed), model);
    }

    [WorkflowState(ApplicationWorkflowState.OnHold)]
    [HttpGet("{applicationId}/on-hold")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> OnHold(Guid applicationId, CancellationToken cancellationToken)
    {
        return await ReturnViewToChangeApplicationStatus(applicationId, cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.OnHold)]
    [HttpPost("{applicationId}/on-hold")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> OnHold(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new HoldApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.HoldReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("OnHold", model)),
            cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.Reactivate)]
    [HttpGet("{applicationId}/reactivate")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Reactivate(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new ReactivateApplicationCommand(AhpApplicationId.From(model.ApplicationId)),
            () => Continue(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("TaskList")),
            cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.RequestToEdit)]
    [HttpGet("{applicationId}/request-to-edit")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> RequestToEdit(Guid applicationId, CancellationToken cancellationToken)
    {
        return await ReturnViewToChangeApplicationStatus(applicationId, cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.RequestToEdit)]
    [HttpPost("{applicationId}/request-to-edit")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> RequestToEdit(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new RequestToEditApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.RequestToEditReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("RequestToEdit", model)),
            cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.Withdraw)]
    [HttpGet("{applicationId}/withdraw")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Withdraw(Guid applicationId, CancellationToken cancellationToken)
    {
        return await ReturnViewToChangeApplicationStatus(applicationId, cancellationToken);
    }

    [WorkflowState(ApplicationWorkflowState.Withdraw)]
    [HttpPost("{applicationId}/withdraw")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Withdraw(string applicationId, ChangeApplicationStatusModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ChangeApplicationStatusModel>(
            _mediator,
            new WithdrawApplicationCommand(AhpApplicationId.From(model.ApplicationId), model.WithdrawReason),
            () => ContinueWithRedirect(new { applicationId }),
            async () => await Task.FromResult<IActionResult>(View("Withdraw", model)),
            cancellationToken);
    }

    [HttpGet("{applicationId}/back")]
    public async Task<IActionResult> Back([FromRoute] string applicationId, ApplicationWorkflowState currentPage)
    {
        return await Back(currentPage, new { applicationId });
    }

    protected override async Task<IStateRouting<ApplicationWorkflowState>> Routing(ApplicationWorkflowState currentState, object? routeData = null)
    {
        var applicationId = this.TryGetApplicationIdFromRoute();
        var isReadOnly = !await _accountAccessContext.CanEditApplication();

        return new ApplicationWorkflow(
                currentState,
                async () => await _mediator.Send(new GetApplicationQuery(applicationId!)),
                async () => applicationId.IsProvided() && await _mediator.Send(new IsApplicationExistQuery(applicationId!)),
                isReadOnly);
    }

    private async Task<IActionResult> ReturnViewToChangeApplicationStatus(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        var model = new ChangeApplicationStatusModel(
            applicationId,
            application.Name);

        return View(model);
    }

    private async Task<ApplicationSubmitModel> GetAplicationSubmitModel(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationDetailsQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        var siteBasicModel = await _mediator.Send(new GetSiteBasicDetailsQuery(application.SiteId.Value), cancellationToken);

        return new ApplicationSubmitModel(
            application.ApplicationId.Value,
            application.ApplicationName,
            application.ReferenceNumber,
            siteBasicModel.Name,
            application.Tenure,
            application.NumberOfHomes.ToString()!,
            CurrencyHelper.DisplayPounds(application.FundingRequested)!,
            CurrencyHelper.DisplayPounds(application.TotalSchemeCost)!,
            application.RepresentationsAndWarranties);
    }
}
