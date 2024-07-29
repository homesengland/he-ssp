using System.Globalization;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.AllocationClaim.Factories;

public class AllocationClaimCheckAnswersViewModelFactory : IAllocationClaimCheckAnswersViewModelFactory
{
    private delegate string CreateAction(string actionName);

    public SectionSummaryViewModel CreateSummary(
        AllocationId allocationId,
        PhaseId phaseId,
        MilestoneClaim claim,
        IUrlHelper urlHelper)
    {
        string CreateAction(string actionName) =>
            CreateAllocationClaimActionUrl(
                urlHelper,
                allocationId,
                phaseId,
                claim.Type,
                actionName);

        return CreateClaimSummary(claim, CreateAction, claim.IsEditable);
    }

    private static SectionSummaryViewModel CreateClaimSummary(MilestoneClaim claim, CreateAction createAction, bool isEditable)
    {
        var items = new List<SectionSummaryItemModel>();

        if (claim.Type == MilestoneType.Acquisition)
        {
            items.Add(new SectionSummaryItemModel(
                "Cost incurred",
                claim.CostsIncurred.MapToYesNo().ToOneElementList(),
                IsEditable: isEditable,
                ActionUrl: createAction(nameof(AllocationClaimController.CostsIncurred))));
        }

        items.Add(new SectionSummaryItemModel(
            $"Amount of grant apportioned to {claim.Type.GetDescription().ToLower(CultureInfo.InvariantCulture)} milestone",
            CurrencyHelper.DisplayPounds(claim.AmountOfGrantApportioned).ToOneElementList(),
            IsEditable: isEditable));

        items.Add(new SectionSummaryItemModel(
            $"{claim.Type.GetDescription()} achievement date",
            FormatDate(claim.AchievementDate),
            createAction(nameof(AllocationClaimController.AchievementDate)),
            IsEditable: isEditable));

        return new SectionSummaryViewModel("Claim summary", items);
    }

    private static string CreateAllocationClaimActionUrl(
        IUrlHelper urlHelper,
        AllocationId allocationId,
        PhaseId phaseId,
        MilestoneType claimType,
        string actionName)
    {
        var action = urlHelper.OrganisationAction(
            actionName,
            new ControllerName(nameof(AllocationClaimController)).WithoutPrefix(),
            new { allocationId = allocationId.Value, phaseId = phaseId.Value, claimType, redirect = "CheckAnswers", });

        return action ?? string.Empty;
    }

    private static IList<string>? FormatDate(DateDetails? date) => DateHelper.DisplayAsUkFormatDate(date)?.ToOneElementList();
}
