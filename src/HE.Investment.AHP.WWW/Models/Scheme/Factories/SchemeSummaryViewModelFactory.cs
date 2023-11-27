using System.Globalization;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Scheme.Factories;

public class SchemeSummaryViewModelFactory : ISchemeSummaryViewModelFactory
{
    private readonly IMediator _mediator;

    public SchemeSummaryViewModelFactory(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<SchemeSummaryViewModel> GetSchemeAndCreateSummary(
        IUrlHelper urlHelper,
        string applicationId,
        CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetApplicationSchemeQuery(applicationId), cancellationToken);

        var items = new List<SectionSummaryItemModel>
        {
            new(
                "Application name",
                scheme.ApplicationName.ToOneElementList(),
                ActionUrl: CreateApplicationActionUrl(urlHelper, scheme.ApplicationId, nameof(ApplicationController.Name))),
            new(
                "Tenure",
                scheme.ApplicationTenure?.GetDescription().ToOneElementList(),
                ActionUrl: CreateApplicationActionUrl(urlHelper, scheme.ApplicationId, nameof(ApplicationController.Tenure))),
            new(
                "Funding required",
                scheme.RequiredFunding.ToWholeNumberString().ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.Funding))),
            new(
                "Number of homes",
                scheme.HousesToDeliver?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.Funding), allowWcagDuplicate: true)),
            new(
                "Affordability od shared ownership",
                scheme.AffordabilityEvidence.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.Affordability))),
            new(
                "Sales risk of shared ownership",
                scheme.SalesRisk.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.SalesRisk))),
            new(
                "Type and tenure of homes",
                scheme.TypeAndTenureJustification.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.HousingNeeds))),
            new(
                "Locally identified housing need",
                scheme.SchemeAndProposalJustification.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.HousingNeeds), allowWcagDuplicate: true)),
            new(
                "Local stakeholder discussions",
                scheme.StakeholderDiscussionsReport.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.StakeholderDiscussions)),
                Files: ConvertFiles(urlHelper, applicationId, scheme.StakeholderDiscussionsFile)),
        };
        return new SchemeSummaryViewModel(scheme.ApplicationId, scheme.ApplicationName, scheme.IsCompleted ? scheme.IsCompleted : null, items);
    }

    private string CreateSchemeActionUrl(IUrlHelper urlHelper, string applicationId, string actionName, bool allowWcagDuplicate = false) =>
        $"{urlHelper.Action(actionName, new ControllerName(nameof(SchemeController)).WithoutPrefix(), new { applicationId, isCheckAnswersMode = true })}{(allowWcagDuplicate ? "#" : string.Empty)}";

    private string CreateApplicationActionUrl(IUrlHelper urlHelper, string applicationId, string actionName) =>
        urlHelper.Action(
            actionName,
            new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
            new { applicationId, callbackUrl = urlHelper.Action("CheckAnswers", "Scheme", new { applicationId }) }) ?? string.Empty;

    private Dictionary<string, string>? ConvertFiles(IUrlHelper urlHelper, string applicationId, UploadedFile? uploadedFile)
    {
        return uploadedFile == null
            ? null
            : new Dictionary<string, string>
            {
                {
                    uploadedFile.FileName, urlHelper.Action(
                        "DownloadStakeholderDiscussionsFile",
                        "Scheme",
                        new { applicationId, fileId = uploadedFile.FileId }) ?? string.Empty
                },
            };
    }
}
