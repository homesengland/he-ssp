using System.Globalization;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Scheme.Factories;

public class SchemeSummaryViewModelFactory : ISchemeSummaryViewModelFactory
{
    public SectionSummaryViewModel GetSchemeAndCreateSummary(string title, Contract.Scheme.Scheme scheme, IUrlHelper urlHelper, bool isReadOnly)
    {
        return new SectionSummaryViewModel(title, new List<SectionSummaryItemModel>
        {
            new(
                "Application name",
                scheme.ApplicationName.ToOneElementList(),
                IsEditable: false),
            new(
                "Tenure",
                scheme.ApplicationTenure?.GetDescription().ToOneElementList(),
                IsEditable: false),
            new(
                "Funding required",
                scheme.RequiredFunding.ToWholeNumberString().ToOneElementList(),
                IsEditable: !isReadOnly,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.Funding))),
            new(
                "Number of homes",
                scheme.HousesToDeliver?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                IsEditable: !isReadOnly,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.Funding), allowWcagDuplicate: true)),
            new(
                "Affordability of Shared Ownership",
                scheme.AffordabilityEvidence.ToOneElementList(),
                IsEditable: !isReadOnly,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.Affordability))),
            new(
                "Sales risk of Shared Ownership",
                scheme.SalesRisk.ToOneElementList(),
                IsEditable: !isReadOnly,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.SalesRisk))),
            new(
                "Type and tenure of homes",
                scheme.MeetingLocalPriorities.ToOneElementList(),
                IsEditable: !isReadOnly,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.HousingNeeds))),
            new(
                "Locally identified housing need",
                scheme.MeetingLocalHousingNeed.ToOneElementList(),
                IsEditable: !isReadOnly,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.HousingNeeds), allowWcagDuplicate: true)),
            new(
                "Local stakeholder discussions",
                scheme.StakeholderDiscussionsReport.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.ApplicationId, nameof(SchemeController.StakeholderDiscussions)),
                IsEditable: !isReadOnly,
                Files: ConvertFiles(urlHelper, scheme.ApplicationId, scheme.StakeholderDiscussionsFile)),
        });
    }

    private static string CreateSchemeActionUrl(IUrlHelper urlHelper, string applicationId, string actionName, bool allowWcagDuplicate = false)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(SchemeController)).WithoutPrefix(),
            new { applicationId, redirect = nameof(SchemeController.CheckAnswers) });

        return $"{action}{(allowWcagDuplicate ? "#" : string.Empty)}";
    }

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
