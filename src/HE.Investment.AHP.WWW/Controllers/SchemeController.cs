using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Common.Queries;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Commands;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investment.AHP.WWW.Models.Scheme.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Contract.Queries;
using HE.Investments.Organisation.ValueObjects;
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

    public SchemeController(IMediator mediator, ISchemeSummaryViewModelFactory summaryViewModelFactory, IAhpDocumentSettings documentSettings)
    {
        _mediator = mediator;
        _summaryViewModelFactory = summaryViewModelFactory;
        _documentSettings = documentSettings;
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
        var scheme = await GetScheme(applicationId, CancellationToken.None);
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
        var scheme = await GetScheme(applicationId, cancellationToken);

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

    [WorkflowState(SchemeWorkflowState.PartnerDetails)]
    [HttpGet("partner-details")]
    public async Task<IActionResult> PartnerDetails([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        return View(await GetScheme(applicationId, cancellationToken));
    }

    [WorkflowState(SchemeWorkflowState.PartnerDetails)]
    [HttpPost("partner-details")]
    public async Task<IActionResult> PartnerDetails([FromRoute] string applicationId, [FromForm] bool? arePartnersConfirmed, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<SchemeViewModel>(
            _mediator,
            new ProvideApplicationPartnersConfirmationCommand(AhpApplicationId.From(applicationId), arePartnersConfirmed),
            async () => await this.ReturnToTaskListOrContinue(async () => await ContinueWithRedirect(new { applicationId })),
            async () => View("PartnerDetails", await GetScheme(applicationId, cancellationToken) with { ArePartnersConfirmed = arePartnersConfirmed }),
            cancellationToken);
    }

    [HttpGet("developing-partner")]
    public async Task<IActionResult> DevelopingPartner([FromRoute] string applicationId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(applicationId, page, cancellationToken));
    }

    [HttpGet("developing-partner-confirm/{organisationId}")]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string applicationId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(applicationId, organisationId, x => x.DevelopingPartner?.OrganisationId, cancellationToken));
    }

    [HttpPost("developing-partner-confirm/{organisationId}")]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string applicationId, [FromRoute] string organisationId, [FromForm] bool? isPartnerConfirmed, [FromQuery] string? redirect, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideDevelopingPartnerCommand(AhpApplicationId.From(applicationId), OrganisationId.From(organisationId), isPartnerConfirmed),
            async () => await this.ReturnToTaskListOrContinue(() =>
                Task.FromResult<IActionResult>(isPartnerConfirmed == true ? RedirectToAction("PartnerDetails", new { applicationId, redirect }) : RedirectToAction("DevelopingPartner", new { applicationId, redirect }))),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(applicationId, organisationId, x => x.DevelopingPartner?.OrganisationId, cancellationToken);
                return View((organisation, isPartnerConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("owner-of-the-land")]
    public async Task<IActionResult> OwnerOfTheLand([FromRoute] string applicationId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(applicationId, page, cancellationToken));
    }

    [HttpGet("owner-of-the-land-confirm/{organisationId}")]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string applicationId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(applicationId, organisationId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken));
    }

    [HttpPost("owner-of-the-land-confirm/{organisationId}")]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string applicationId, [FromRoute] string organisationId, [FromForm] bool? isPartnerConfirmed, [FromQuery] string? redirect, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheLandCommand(AhpApplicationId.From(applicationId), OrganisationId.From(organisationId), isPartnerConfirmed),
            async () => await this.ReturnToTaskListOrContinue(() =>
                Task.FromResult<IActionResult>(isPartnerConfirmed == true ? RedirectToAction("PartnerDetails", new { applicationId, redirect }) : RedirectToAction("OwnerOfTheLand", new { applicationId, redirect }))),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(applicationId, organisationId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken);
                return View((organisation, isPartnerConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("owner-of-the-homes")]
    public async Task<IActionResult> OwnerOfTheHomes([FromRoute] string applicationId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(applicationId, page, cancellationToken));
    }

    [HttpGet("owner-of-the-homes-confirm/{organisationId}")]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string applicationId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(applicationId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken));
    }

    [HttpPost("owner-of-the-homes-confirm/{organisationId}")]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string applicationId, [FromRoute] string organisationId, [FromForm] bool? isPartnerConfirmed, [FromQuery] string? redirect, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheHomesCommand(AhpApplicationId.From(applicationId), OrganisationId.From(organisationId), isPartnerConfirmed),
            async () => await this.ReturnToTaskListOrContinue(() =>
                Task.FromResult<IActionResult>(isPartnerConfirmed == true ? RedirectToAction("PartnerDetails", new { applicationId, redirect }) : RedirectToAction("OwnerOfTheHomes", new { applicationId, redirect }))),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(applicationId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken);
                return View((organisation, isPartnerConfirmed));
            },
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpGet("affordability")]
    public async Task<IActionResult> Affordability([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await GetScheme(applicationId, cancellationToken);

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
        var scheme = await GetScheme(applicationId, cancellationToken);

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
        var scheme = await GetScheme(applicationId, cancellationToken);

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
        var scheme = await GetScheme(applicationId, cancellationToken, includeFiles: true);

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

        var scheme = await GetScheme(applicationId, CancellationToken.None);
        return await Task.FromResult(new SchemeWorkflow(currentState, scheme));
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
            scheme.Application.Name,
            CurrencyHelper.InputPounds(scheme.RequiredFunding),
            scheme.HousesToDeliver?.ToString(CultureInfo.InvariantCulture),
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
        var scheme = await GetScheme(applicationId, cancellationToken, includeFiles: true);
        var section = _summaryViewModelFactory.GetSchemeAndCreateSummary("Scheme information", scheme, urlHelper);

        return new SchemeSummaryViewModel(
            scheme.Application.Id.Value,
            scheme.Application.Name,
            scheme.Status == SectionStatus.Completed ? IsSectionCompleted.Yes : null,
            section,
            scheme.Application.AllowedOperations);
    }

    private async Task<Scheme> GetScheme(string applicationId, CancellationToken cancellationToken, bool includeFiles = false)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(AhpApplicationId.From(applicationId), includeFiles), cancellationToken);
        ViewBag.ApplicationName = scheme.Application.Name;

        return scheme;
    }

    private async Task<SelectPartnerModel> GetSelectPartnerModel(string applicationId, int? page, CancellationToken cancellationToken)
    {
        var scheme = await GetScheme(applicationId, cancellationToken);
        var partners = await _mediator.Send(new GetConsortiumMembersQuery(new PaginationRequest(page ?? 1)), cancellationToken);

        return new SelectPartnerModel(scheme.Application.Id.Value, scheme.Application.Name, partners);
    }

    private async Task<(OrganisationDetails Organisation, bool? IsConfirmed)> GetConfirmPartnerModel(
        string applicationId,
        string organisationId,
        Func<Scheme, string?> getSelectedPartnerId,
        CancellationToken cancellationToken)
    {
        var scheme = await GetScheme(applicationId, cancellationToken);
        var organisationDetails = await _mediator.Send(new GetOrganisationDetailsQuery(new OrganisationIdentifier(organisationId)), cancellationToken);
        var currentlySelectedPartner = getSelectedPartnerId(scheme);

        return (organisationDetails, organisationDetails.OrganisationId == currentlySelectedPartner ? true : null);
    }
}
