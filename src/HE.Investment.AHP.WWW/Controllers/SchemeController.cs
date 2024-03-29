using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using HE.Investment.AHP.WWW.Utils;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/scheme")]
public class SchemeController : WorkflowController<SchemeWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly ISchemeSummaryViewModelFactory _summaryViewModelFactory;
    private readonly IAhpDocumentSettings _documentSettings;
    private readonly ISchemeProvider _schemeProvider;
    private readonly IAccountAccessContext _accountAccessContext;

    public SchemeController(
        IMediator mediator,
        ISchemeSummaryViewModelFactory summaryViewModelFactory,
        IAhpDocumentSettings documentSettings,
        ISchemeProvider schemeProvider,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _summaryViewModelFactory = summaryViewModelFactory;
        _documentSettings = documentSettings;
        _schemeProvider = schemeProvider;
        _accountAccessContext = accountAccessContext;
    }

    [WorkflowState(SchemeWorkflowState.Start)]
    [HttpGet("start")]
    public async Task<IActionResult> Start([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View("Start", application.Name);
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back([FromRoute] string applicationId, SchemeWorkflowState currentPage)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId)), CancellationToken.None);
        if (currentPage == SchemeWorkflowState.Funding && scheme.Status == SectionStatus.InProgress)
        {
            return RedirectToAction("TaskList", "Application", new { applicationId });
        }

        return await Back(currentPage, new { applicationId });
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpGet("funding")]
    public async Task<IActionResult> Funding([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View("Funding", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpPost("funding")]
    public async Task<IActionResult> Funding(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeFundingCommand(AhpApplicationId.From(model.ApplicationId), model.RequiredFunding, model.HousesToDeliver),
            model.ApplicationId,
            nameof(Funding),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpGet("affordability")]
    public async Task<IActionResult> Affordability([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View("Affordability", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpPost("affordability")]
    public async Task<IActionResult> Affordability(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeAffordabilityCommand(AhpApplicationId.From(model.ApplicationId), model.AffordabilityEvidence),
            model.ApplicationId,
            nameof(Affordability),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.SalesRisk)]
    [HttpGet("sales-risk")]
    public async Task<IActionResult> SalesRisk([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View("SalesRisk", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.SalesRisk)]
    [HttpPost("sales-risk")]
    public async Task<IActionResult> SalesRisk(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeSalesRiskCommand(AhpApplicationId.From(model.ApplicationId), model.SalesRisk),
            model.ApplicationId,
            nameof(SalesRisk),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.HousingNeeds)]
    [HttpGet("housing-needs")]
    public async Task<IActionResult> HousingNeeds([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View("HousingNeeds", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.HousingNeeds)]
    [HttpPost("housing-needs")]
    public async Task<IActionResult> HousingNeeds(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeHousingNeedsCommand(
                AhpApplicationId.From(model.ApplicationId),
                model.MeetingLocalPriorities,
                model.MeetingLocalHousingNeed),
            model.ApplicationId,
            nameof(HousingNeeds),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.StakeholderDiscussions)]
    [HttpGet("stakeholder-discussions")]
    public async Task<IActionResult> StakeholderDiscussions([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId), true), cancellationToken);

        return View("StakeholderDiscussions", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.StakeholderDiscussions)]
    [HttpPost("stakeholder-discussions")]
    public async Task<IActionResult> StakeholderDiscussions(SchemeViewModel model, CancellationToken cancellationToken)
    {
        var fileToUpload = model.LocalAuthoritySupportFile == null
            ? null
            : new FileToUpload(
                model.LocalAuthoritySupportFile.FileName,
                model.LocalAuthoritySupportFile.Length,
                model.LocalAuthoritySupportFile.OpenReadStream());

        try
        {
            return await ExecuteCommand(
                new ChangeSchemeStakeholderDiscussionsCommand(
                    AhpApplicationId.From(model.ApplicationId),
                    model.StakeholderDiscussionsReport,
                    fileToUpload),
                model.ApplicationId,
                nameof(StakeholderDiscussions),
                model,
                cancellationToken);
        }
        finally
        {
            if (fileToUpload != null)
            {
                await fileToUpload.Content.DisposeAsync();
            }
        }
    }

    [WorkflowState(SchemeWorkflowState.StakeholderDiscussions)]
    [HttpGet("remove-stakeholder-discussions-file")]
    public async Task<IActionResult> RemoveStakeholderDiscussionsFile(
        [FromRoute] string applicationId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RemoveStakeholderDiscussionsFileCommand(AhpApplicationId.From(applicationId), FileId.From(fileId)),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            throw new DomainValidationException(result);
        }

        return RedirectToAction("StakeholderDiscussions", new { applicationId });
    }

    [HttpGet("download-stakeholder-discussions-file")]
    public async Task<IActionResult> DownloadStakeholderDiscussionsFile(
        [FromRoute] string applicationId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetStakeholderDiscussionsFileQuery(AhpApplicationId.From(applicationId), FileId.From(fileId)),
            cancellationToken);

        return File(response.Content, "application/octet-stream", response.Name);
    }

    [WorkflowState(SchemeWorkflowState.CheckAnswers)]
    [HttpGet("check-answers")]
    public async Task<IActionResult> CheckAnswers([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        return View("CheckAnswers", await GetSchemeAndCreateSummary(Url, applicationId, cancellationToken));
    }

    [WorkflowState(SchemeWorkflowState.CheckAnswers)]
    [HttpPost("check-answers")]
    public async Task<IActionResult> Complete(
        [FromRoute] string applicationId,
        [FromForm] IsSectionCompleted? isCompleted,
        string? action,
        CancellationToken cancellationToken)
    {
        if (isCompleted == null)
        {
            ModelState.AddModelError(nameof(SchemeSummaryViewModel.IsCompleted), "Select whether you have completed this section");
            return View("CheckAnswers", await GetSchemeAndCreateSummary(Url, applicationId, cancellationToken));
        }

        if (isCompleted == IsSectionCompleted.Yes)
        {
            var result = await _mediator.Send(new CompleteSchemeCommand(AhpApplicationId.From(applicationId)), cancellationToken);
            if (result.HasValidationErrors)
            {
                ModelState.AddModelError(
                    nameof(SchemeSummaryViewModel.IsCompleted),
                    "You have not completed this section. Select no if you want to come back later");
                return View("CheckAnswers", await GetSchemeAndCreateSummary(Url, applicationId, cancellationToken));
            }
        }
        else
        {
            var result = await _mediator.Send(new UnCompleteSchemeCommand(AhpApplicationId.From(applicationId)), cancellationToken);
            if (result.HasValidationErrors)
            {
                ModelState.AddModelError(
                    nameof(SchemeSummaryViewModel.IsCompleted),
                    "You cannot change status for completed section.");
                return View("CheckAnswers", await GetSchemeAndCreateSummary(Url, applicationId, cancellationToken));
            }
        }

        return Url.RedirectToTaskList(applicationId);
    }

    protected override async Task<IStateRouting<SchemeWorkflowState>> Routing(SchemeWorkflowState currentState, object? routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                            ?? routeData?.GetPropertyValue<string>("applicationId")
                            ?? throw new NotFoundException($"{nameof(SchemeController)} required applicationId path parameter.");

        if (string.IsNullOrEmpty(applicationId))
        {
            throw new InvalidOperationException("Cannot find applicationId.");
        }

        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId)), CancellationToken.None);
        var isReadOnly = !await _accountAccessContext.CanEditApplication() || scheme.IsReadOnly;
        return await Task.FromResult(new SchemeWorkflow(currentState, scheme, isReadOnly));
    }

    private SchemeViewModel CreateModel(string applicationId, Scheme scheme)
    {
        string GetRemoveAction(FileId fileId) =>
            Url.Action("RemoveStakeholderDiscussionsFile", "Scheme", new { applicationId, fileId = fileId.Value }) ?? string.Empty;

        IList<FileModel>? CreateFileModel(UploadedFile? x) =>
            x == null
                ? null
                : new List<FileModel> { new(x.FileId.Value, x.FileName, x.UploadedOn, x.UploadedBy, x.CanBeRemoved, GetRemoveAction(x.FileId), string.Empty) };

        return new SchemeViewModel(
            applicationId,
            scheme.ApplicationName,
            CurrencyHelper.InputPounds(scheme.RequiredFunding),
            scheme.HousesToDeliver.ToString(),
            scheme.AffordabilityEvidence,
            scheme.SalesRisk,
            scheme.MeetingLocalPriorities,
            scheme.MeetingLocalHousingNeed,
            scheme.StakeholderDiscussionsReport,
            CreateFileModel(scheme.LocalAuthoritySupportFile),
            _documentSettings.MaxFileSize.Megabytes,
            string.Join(", ", _documentSettings.AllowedExtensions.Select(x => x.Value.ToUpperInvariant())));
    }

    private async Task<IActionResult> ExecuteCommand(
        IRequest<OperationResult> command,
        string applicationId,
        string viewName,
        SchemeViewModel model,
        CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<SchemeViewModel>(
            _mediator,
            command,
            async () => await this.ReturnToTaskListOrContinue(async () => await ContinueWithRedirect(new { applicationId })),
            async () => await Task.FromResult<IActionResult>(View(viewName, model)),
            cancellationToken);
    }

    private async Task<SchemeSummaryViewModel> GetSchemeAndCreateSummary(
        IUrlHelper urlHelper,
        string applicationId,
        CancellationToken cancellationToken)
    {
        var scheme = await _schemeProvider.Get(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId), true), cancellationToken);
        var isEditable = await _accountAccessContext.CanEditApplication() && !scheme.IsReadOnly;
        var section = _summaryViewModelFactory.GetSchemeAndCreateSummary("Scheme information", scheme, urlHelper, !isEditable);

        return new SchemeSummaryViewModel(
            scheme.ApplicationId.Value,
            scheme.ApplicationName,
            scheme.Status == SectionStatus.Completed ? IsSectionCompleted.Yes : null,
            section,
            isEditable,
            scheme.IsApplicationLocked);
    }
}
