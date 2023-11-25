using System.Globalization;
using System.Security.Policy;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Workflows;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UploadedFile = HE.Investment.AHP.Contract.Common.UploadedFile;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/scheme")]
public class SchemeController : WorkflowController<SchemeWorkflowState>
{
    private readonly IMediator _mediator;

    public SchemeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [WorkflowState(SchemeWorkflowState.Index)]
    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

        return View("Index", application.Name);
    }

    [HttpGet("back")]
    public Task<IActionResult> Back([FromRoute] string applicationId, SchemeWorkflowState currentPage)
    {
        return Back(currentPage, new { applicationId });
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpGet("funding")]
    public async Task<IActionResult> Funding([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        return View("Funding", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpPost("funding")]
    public async Task<IActionResult> Funding(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeFundingCommand(model.ApplicationId, model.RequiredFunding, model.HousesToDeliver),
            model.ApplicationId,
            nameof(Funding),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpGet("affordability")]
    public async Task<IActionResult> Affordability([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        return View("Affordability", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpPost("affordability")]
    public async Task<IActionResult> Affordability(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeAffordabilityCommand(model.ApplicationId, model.AffordabilityEvidence),
            model.ApplicationId,
            nameof(Affordability),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.SalesRisk)]
    [HttpGet("sales-risk")]
    public async Task<IActionResult> SalesRisk([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        return View("SalesRisk", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.SalesRisk)]
    [HttpPost("sales-risk")]
    public async Task<IActionResult> SalesRisk(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeSalesRiskCommand(model.ApplicationId, model.SalesRisk ?? Check.IfCanBeNull),
            model.ApplicationId,
            nameof(SalesRisk),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.HousingNeeds)]
    [HttpGet("housing-needs")]
    public async Task<IActionResult> HousingNeeds([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        return View("HousingNeeds", CreateModel(applicationId, scheme));
    }

    [WorkflowState(SchemeWorkflowState.HousingNeeds)]
    [HttpPost("housing-needs")]
    public async Task<IActionResult> HousingNeeds(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeHousingNeedsCommand(model.ApplicationId, model.TypeAndTenureJustification ?? Check.IfCanBeNull, model.SchemeAndProposalJustification ?? Check.IfCanBeNull),
            model.ApplicationId,
            nameof(HousingNeeds),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.StakeholderDiscussions)]
    [HttpGet("stakeholder-discussions")]
    public async Task<IActionResult> StakeholderDiscussions([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        return View("StakeholderDiscussions", CreateModel(applicationId, scheme));
    }

    [DisableRequestSizeLimit]
    [WorkflowState(SchemeWorkflowState.StakeholderDiscussions)]
    [HttpPost("stakeholder-discussions")]
    public async Task<IActionResult> StakeholderDiscussions(SchemeViewModel model, CancellationToken cancellationToken)
    {
        var filesToUpload = (model.StakeholderDiscussionFiles ?? new List<IFormFile>())
            .Select(x => new FileToUpload(x.FileName, x.Length, x.OpenReadStream()))
            .ToList();

        try
        {
            return await ExecuteCommand(
                new ChangeSchemeStakeholderDiscussionsCommand(model.ApplicationId, model.StakeholderDiscussionsReport ?? Check.IfCanBeNull, filesToUpload),
                model.ApplicationId,
                nameof(StakeholderDiscussions),
                model,
                cancellationToken);
        }
        finally
        {
            foreach (var file in filesToUpload)
            {
                await file.Content.DisposeAsync();
            }
        }
    }

    [WorkflowState(SchemeWorkflowState.StakeholderDiscussions)]
    [HttpGet("RemoveStakeholderDiscussionsFile")]
    public async Task<IActionResult> RemoveStakeholderDiscussionsFile(
        [FromRoute] string applicationId,
        [FromQuery] string fileId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveStakeholderDiscussionsFileCommand(applicationId, fileId), cancellationToken);
        if (result.HasValidationErrors)
        {
            throw new DomainValidationException(result);
        }

        return RedirectToAction("StakeholderDiscussions", new { applicationId });
    }

    [WorkflowState(SchemeWorkflowState.Summary)]
    [HttpGet("summary")]
    public async Task<IActionResult> Summary([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        return View("Summary", (scheme.ApplicationName, Items: CreateSummaryModel(scheme)));
    }

    protected override async Task<IStateRouting<SchemeWorkflowState>> Routing(SchemeWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult(new SchemeWorkflow(currentState));
    }

    private SchemeViewModel CreateModel(string applicationId, Scheme scheme)
    {
        string GetRemoveAction(string fileId) =>
            Url.RouteUrl("section", new { controller = "scheme", action = "RemoveStakeholderDiscussionsFile", applicationId, fileId }) ?? Check.IfCanBeNull;

        FileModel CreateFileModel(UploadedFile x) => new(x.FileId, x.FileName, x.UploadedOn, x.UploadedBy, x.CanBeRemoved, GetRemoveAction(x.FileId));

        return new SchemeViewModel(
            applicationId,
            scheme.ApplicationName,
            scheme.RequiredFunding.ToString(),
            scheme.HousesToDeliver.ToString(),
            scheme.AffordabilityEvidence,
            scheme.SalesRisk,
            scheme.TypeAndTenureJustification,
            scheme.SchemeAndProposalJustification,
            scheme.StakeholderDiscussionsReport,
            scheme.StakeholderDiscussionsFiles.Select(CreateFileModel).ToList(),
            new List<IFormFile>());
    }

    private async Task<IActionResult> ExecuteCommand(
        IRequest<OperationResult> command,
        string applicationId,
        string viewName,
        object model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(viewName, model);
        }

        return await Continue(new { applicationId });
    }

    private IList<SectionSummaryItemModel> CreateSummaryModel(Scheme scheme)
    {
        return new List<SectionSummaryItemModel>
        {
            new(
                "Funding requested",
                scheme.RequiredFunding?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(Funding), scheme.ApplicationId)),
            new(
                "Number of homes",
                scheme.HousesToDeliver?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(Funding), scheme.ApplicationId)),
            new(
                "Affordability od shared ownership",
                scheme.AffordabilityEvidence.ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(Affordability), scheme.ApplicationId)),
            new(
                "Sales risk of shared ownership",
                scheme.SalesRisk.ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(SalesRisk), scheme.ApplicationId)),
            new(
                "Type and tenure of homes",
                scheme.TypeAndTenureJustification.ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(SalesRisk), scheme.ApplicationId)),
            new(
                "Locally identified housing need",
                scheme.SchemeAndProposalJustification.ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(HousingNeeds), scheme.ApplicationId)),
            new(
                "Local stakeholder discussions",
                scheme.StakeholderDiscussionsReport.ToOneElementList(),
                ActionUrl: CreateActionUrl(nameof(StakeholderDiscussions), scheme.ApplicationId),
                Files: scheme.StakeholderDiscussionsFiles.ToDictionary(f => f.FileName, f => f.FileId)),
        };
    }

    private string CreateActionUrl(string actionName, string applicationId) =>
        Url.Action(actionName, "Scheme", new { applicationId }) ?? throw new InvalidOperationException();
}
