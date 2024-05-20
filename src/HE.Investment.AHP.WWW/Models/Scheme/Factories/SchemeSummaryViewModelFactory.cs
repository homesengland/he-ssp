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
        return new SectionSummaryViewModel(title, CreateSummaryItems(scheme, urlHelper).ToList());
    }

    private static string CreateSchemeActionUrl(IUrlHelper urlHelper, AhpApplicationId applicationId, string actionName, bool allowWcagDuplicate = false)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(SchemeController)).WithoutPrefix(),
            new { applicationId = applicationId.Value, redirect = nameof(SchemeController.CheckAnswers) });

        return $"{action}{(allowWcagDuplicate ? "#" : string.Empty)}";
    }

    private IEnumerable<SectionSummaryItemModel> CreateSummaryItems(Contract.Scheme.Scheme scheme, IUrlHelper urlHelper)
    {
        yield return new(
            "Application name",
            scheme.Application.Name.ToOneElementList(),
            IsEditable: false);
        yield return new(
            "Tenure",
            scheme.Application.Tenure.GetDescription().ToOneElementList(),
            IsEditable: false);
        yield return new(
            "Funding required",
            scheme.RequiredFunding.DisplayPounds().ToOneElementList(),
            IsEditable: scheme.Application.IsEditable,
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.Funding)));
        yield return new(
            "Number of homes",
            scheme.HousesToDeliver?.ToString(CultureInfo.InvariantCulture).ToOneElementList(),
            IsEditable: scheme.Application.IsEditable,
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.Funding), allowWcagDuplicate: true));

        if (scheme.IsConsortiumMember)
        {
            yield return new(
                "Developing partner",
                scheme.DevelopingPartner?.Name.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.DevelopingPartner)));
            yield return new(
                "Owner of the land",
                scheme.OwnerOfTheLand?.Name.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.OwnerOfTheLand)));
            yield return new(
                "Owner of the homes",
                scheme.OwnerOfTheHomes?.Name.ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.OwnerOfTheHomes)));
            yield return new(
                "Are partner details correct?",
                scheme.ArePartnersConfirmed.MapToYesNo().ToOneElementList(),
                IsEditable: scheme.Application.IsEditable,
                ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.PartnerDetails)));
        }

        yield return new(
            "Affordability of Shared Ownership",
            scheme.AffordabilityEvidence.ToOneElementList(),
            IsEditable: scheme.Application.IsEditable,
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.Affordability)));
        yield return new(
            "Sales risk of Shared Ownership",
            scheme.SalesRisk.ToOneElementList(),
            IsEditable: scheme.Application.IsEditable,
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.SalesRisk)));
        yield return new(
            "Type and tenure of homes",
            scheme.MeetingLocalPriorities.ToOneElementList(),
            IsEditable: scheme.Application.IsEditable,
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.HousingNeeds)));
        yield return new(
            "Locally identified housing need",
            scheme.MeetingLocalHousingNeed.ToOneElementList(),
            IsEditable: scheme.Application.IsEditable,
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.HousingNeeds), allowWcagDuplicate: true));
        yield return new(
            "Local stakeholder discussions",
            scheme.StakeholderDiscussionsReport.ToOneElementList(),
            ActionUrl: CreateSchemeActionUrl(urlHelper, scheme.Application.Id, nameof(SchemeController.StakeholderDiscussions)),
            IsEditable: scheme.Application.IsEditable,
            Files: ConvertFiles(urlHelper, scheme.Application.Id, scheme.LocalAuthoritySupportFile));
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
