using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Scheme.Factories;

public class SchemeSummaryViewModelFactory : ISchemeSummaryViewModelFactory
{
    public SectionSummaryViewModel GetSchemeAndCreateSummary(string title, Contract.Scheme.Scheme scheme, IUrlHelper urlHelper)
    {
        return new SectionSummaryViewModel(title, new List<SectionSummaryItemModel>
        {
            new(
                "Application name",
                scheme.Application.Name.ToOneElementList(),
                IsEditable: false),
            new(
                "Tenure",
                scheme.Application.Tenure.GetDescription().ToOneElementList(),
                IsEditable: false),
            new(
                "Funding required",
                scheme.RequiredFunding.DisplayPounds().ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.Funding))),
            new(
                "Number of homes",
                scheme.HousesToDeliver?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.Funding), allowWcagDuplicate: true)),
            new(
                "Affordability of Shared Ownership",
                scheme.AffordabilityEvidence.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.Affordability))),
            new(
                "Sales risk of Shared Ownership",
                scheme.SalesRisk.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.SalesRisk))),
            new(
                "Type and tenure of homes",
                scheme.MeetingLocalPriorities.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.HousingNeeds))),
            new(
                "Locally identified housing need",
                scheme.MeetingLocalHousingNeed.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.HousingNeeds), allowWcagDuplicate: true)),
            new(
                "Local stakeholder discussions",
                scheme.StakeholderDiscussionsReport.ToOneElementList(),
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.StakeholderDiscussions)),
                IsEditable: scheme.Application.IsEditable,
                Files: ConvertFiles(urlHelper, scheme.Application.Id, scheme.LocalAuthoritySupportFile)),
        });
    }

    private static string CreateSchemeActionUrl(IUrlHelper urlHelper, AhpApplicationId applicationId, string actionName, bool allowWcagDuplicate = false)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(SchemeController)).WithoutPrefix(),
            new { applicationId = applicationId.Value, redirect = nameof(SchemeController.CheckAnswers) });

        return $"{action}{(allowWcagDuplicate ? "#" : string.Empty)}";
    }

    private Dictionary<string, string>? ConvertFiles(IUrlHelper urlHelper, AhpApplicationId applicationId, UploadedFile? uploadedFile)
    {
        return uploadedFile == null
            ? null
            : new Dictionary<string, string>
            {
                {
                    uploadedFile.FileName, urlHelper.Action(
                        "DownloadStakeholderDiscussionsFile",
                        "Scheme",
                        new { applicationId = applicationId.Value, fileId = uploadedFile.FileId.Value }) ?? string.Empty
                },
            };
    }
}
