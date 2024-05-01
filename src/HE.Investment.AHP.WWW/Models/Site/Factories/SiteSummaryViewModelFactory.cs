using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components.SectionSummary;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Models.Summary;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;
using Controller = HE.Investment.AHP.WWW.Controllers.SiteController;

namespace HE.Investment.AHP.WWW.Models.Site.Factories;

public class SiteSummaryViewModelFactory : ISiteSummaryViewModelFactory
{
    private delegate string CreateAction(string actionName);

    public IEnumerable<SectionSummaryViewModel> CreateSiteSummary(SiteModel siteDetails, IUrlHelper urlHelper, bool isEditable, bool useWorkflowRedirection)
    {
        string CreateAction(string actionName) => CreateSiteActionUrl(urlHelper, SiteId.From(siteDetails.Id!), actionName, useWorkflowRedirection);

        yield return new SectionSummaryViewModel("Site details", CreateSiteDetailsSummary(siteDetails, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Section 106", CreateSection106Summary(siteDetails.Section106, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Location", CreateLocationSummary(siteDetails.LocalAuthority, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Planning", CreatePlanningSummary(siteDetails.PlanningDetails, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Design guidelines", CreateDesignGuidelinesSummary(siteDetails, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Consortium", CreateConsortiumSummary());
        yield return new SectionSummaryViewModel("URB", CreateUrbSummary());
        yield return new SectionSummaryViewModel("Land details", CreateLandDetailsSummary(siteDetails, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Site use", CreateSiteUseSummary(siteDetails, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Modern Methods of Construction (MMC)", CreateMmcSummary(siteDetails.ModernMethodsOfConstruction, CreateAction, isEditable));
        yield return new SectionSummaryViewModel("Procurement", CreateProcurementSummary(siteDetails, CreateAction, isEditable));
    }

    private static IList<SectionSummaryItemModel> CreateSiteDetailsSummary(SiteModel site, CreateAction createAction, bool isEditable)
    {
        return new List<SectionSummaryItemModel>
        {
            new("Site name", site.Name.ToOneElementList(), createAction(nameof(Controller.Name)), IsEditable: isEditable),
        };
    }

    private static IList<SectionSummaryItemModel> CreateSection106Summary(Section106Dto? section, CreateAction createAction, bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new(
                "106 agreement",
                SummaryAnswerHelper.ToYesNo(section?.GeneralAgreement),
                createAction(nameof(Controller.Section106GeneralAgreement)),
                IsEditable: isEditable),
        };

        summary.AddWhen(
            new(
                "Secure delivery through developer contributions",
                SummaryAnswerHelper.ToYesNo(section?.AffordableHousing),
                createAction(nameof(Controller.Section106AffordableHousing)),
                IsEditable: isEditable),
            section?.GeneralAgreement == true);
        summary.AddWhen(
            new(
                "100% affordable housing",
                SummaryAnswerHelper.ToYesNo(section?.OnlyAffordableHousing),
                createAction(nameof(Controller.Section106OnlyAffordableHousing)),
                IsEditable: isEditable),
            section?.AffordableHousing == true);
        summary.AddWhen(
            new(
                "Additional affordable housing",
                SummaryAnswerHelper.ToYesNo(section?.AdditionalAffordableHousing),
                createAction(nameof(Controller.Section106AdditionalAffordableHousing)),
                IsEditable: isEditable),
            section?.OnlyAffordableHousing == false);
        summary.AddWhen(
            new(
                "Capital funding guide eligibility",
                SummaryAnswerHelper.ToYesNo(section?.CapitalFundingEligibility),
                createAction(nameof(Controller.Section106CapitalFundingEligibility)),
                IsEditable: isEditable),
            section?.GeneralAgreement == true);
        summary.AddWhen(
            new(
                "Local authority confirmation",
                section?.LocalAuthorityConfirmation.ToOneElementList(),
                createAction(nameof(Controller.Section106LocalAuthorityConfirmation)),
                IsEditable: isEditable),
            section?.AdditionalAffordableHousing == true);

        return summary;
    }

    private static IList<SectionSummaryItemModel> CreateLocationSummary(LocalAuthority? localAuthority, CreateAction createAction, bool isEditable)
    {
        return new List<SectionSummaryItemModel>
        {
            new(
                "Local authority",
                localAuthority?.Name.ToOneElementList(),
                createAction(nameof(Controller.LocalAuthoritySearch)),
                IsEditable: isEditable),
        };
    }

    private static IList<SectionSummaryItemModel> CreatePlanningSummary(SitePlanningDetails planning, CreateAction createAction, bool isEditable)
    {
        var detailsAction = createAction(nameof(Controller.PlanningDetails));
        var summary = new List<SectionSummaryItemModel>
        {
            new("Planning status", SummaryAnswerHelper.ToEnum(planning.PlanningStatus), createAction(nameof(Controller.PlanningStatus)), IsEditable: isEditable),
        };

        summary.AddWhen(
            new SectionSummaryItemModel("Planning reference number", planning.ReferenceNumber.ToOneElementList(), detailsAction, IsEditable: isEditable),
            planning.IsReferenceNumberActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Date application for outline planning permission submitted",
                SummaryAnswerHelper.ToDate(planning.PlanningSubmissionDate),
                detailsAction,
                IsEditable: isEditable),
            planning.IsPlanningSubmissionDateActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Date outline planning approval granted",
                SummaryAnswerHelper.ToDate(planning.OutlinePlanningApprovalDate),
                detailsAction,
                IsEditable: isEditable),
            planning.IsOutlinePlanningApprovalDateActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Date detailed planning approval granted",
                SummaryAnswerHelper.ToDate(planning.DetailedPlanningApprovalDate),
                detailsAction,
                IsEditable: isEditable),
            planning.IsDetailedPlanningApprovalDateActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Date application for detailed planning submitted",
                SummaryAnswerHelper.ToDate(planning.ApplicationForDetailedPlanningSubmittedDate),
                detailsAction,
                IsEditable: isEditable),
            planning.IsApplicationForDetailedPlanningSubmittedDateActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Expected detailed planning approval date",
                SummaryAnswerHelper.ToDate(planning.ExpectedPlanningApprovalDate),
                detailsAction,
                IsEditable: isEditable),
            planning.IsExpectedPlanningApprovalDateActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "All the homes covered by planning application",
                SummaryAnswerHelper.ToYesNo(planning.IsGrantFundingForAllHomesCoveredByApplication),
                detailsAction,
                IsEditable: isEditable),
            planning.IsGrantFundingForAllHomesCoveredByApplicationActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Further steps required",
                planning.RequiredFurtherSteps.ToOneElementList(),
                detailsAction,
                IsEditable: isEditable),
            planning.IsRequiredFurtherStepsActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Registered title to the land",
                SummaryAnswerHelper.ToYesNo(planning.IsLandRegistryTitleNumberRegistered),
                detailsAction,
                IsEditable: isEditable),
            planning.IsLandRegistryActive);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Land Registry title number",
                planning.LandRegistryTitleNumber.ToOneElementList(),
                createAction(nameof(Controller.LandRegistry)),
                IsEditable: isEditable),
            planning is { IsLandRegistryActive: true, IsLandRegistryTitleNumberRegistered: true });
        summary.AddWhen(
            new SectionSummaryItemModel(
                "All the homes covered by title number",
                SummaryAnswerHelper.ToYesNo(planning.IsGrantFundingForAllHomesCoveredByTitleNumber),
                createAction(nameof(Controller.LandRegistry)),
                IsEditable: isEditable),
            planning is { IsLandRegistryActive: true, IsLandRegistryTitleNumberRegistered: true });

        return summary;
    }

    private static IList<SectionSummaryItemModel> CreateDesignGuidelinesSummary(SiteModel site, CreateAction createAction, bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new(
                "National Design Guide priorities",
                site.NationalDesignGuidePriorities.Select(x => x.GetDescription()).ToList(),
                createAction(nameof(Controller.NationalDesignGuide)),
                IsEditable: isEditable),
            new(
                "Building for a Healthy Life criteria",
                SummaryAnswerHelper.ToEnum(site.BuildingForHealthyLife),
                createAction(nameof(Controller.BuildingForHealthyLife)),
                IsEditable: isEditable),
        };

        summary.AddWhen(
            new SectionSummaryItemModel(
                "Number of green lights",
                site.NumberOfGreenLights.ToOneElementList(),
                createAction(nameof(Controller.NumberOfGreenLights)),
                IsEditable: isEditable),
            site.BuildingForHealthyLife == BuildingForHealthyLifeType.Yes);

        return summary;
    }

    private static IList<SectionSummaryItemModel> CreateConsortiumSummary()
    {
        // TODO: AB#65903: Site information - Partner information
        return new List<SectionSummaryItemModel>
        {
            new("Developing partner", "TODO".ToOneElementList()),
            new("Owner of the land", "TODO".ToOneElementList()),
            new("Owner of the homes", "TODO".ToOneElementList()),
        };
    }

    private static IList<SectionSummaryItemModel> CreateUrbSummary()
    {
        // TODO: AB#65903: Site information - Partner information
        return new List<SectionSummaryItemModel> { new("Owner of the homes", "TODO".ToOneElementList()) };
    }

    private static IList<SectionSummaryItemModel> CreateLandDetailsSummary(SiteModel site, CreateAction createAction, bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new(
                "Land status",
                SummaryAnswerHelper.ToEnum(site.LandAcquisitionStatus),
                createAction(nameof(Controller.LandAcquisitionStatus)),
                IsEditable: isEditable),
            new(
                "Tendering progress for main works contract",
                SummaryAnswerHelper.ToEnum(site.TenderingStatusDetails.TenderingStatus),
                createAction(nameof(Controller.TenderingStatus)),
                IsEditable: isEditable),
        };

        if (site.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.UnconditionalWorksContract or SiteTenderingStatus.ConditionalWorksContract)
        {
            summary.Add(new SectionSummaryItemModel(
                "Name of contractor",
                site.TenderingStatusDetails.ContractorName.ToOneElementList(),
                createAction(nameof(Controller.ContractorDetails)),
                IsEditable: isEditable));
            summary.Add(new SectionSummaryItemModel(
                "Contractor SME",
                SummaryAnswerHelper.ToYesNo(site.TenderingStatusDetails.IsSmeContractor),
                createAction(nameof(Controller.ContractorDetails)),
                IsEditable: isEditable));
        }

        summary.AddWhen(
            new SectionSummaryItemModel(
                "Intention to work with SME contractor",
                SummaryAnswerHelper.ToYesNo(site.TenderingStatusDetails.IsIntentionToWorkWithSme),
                createAction(nameof(Controller.IntentionToWorkWithSme)),
                IsEditable: isEditable),
            site.TenderingStatusDetails.TenderingStatus is SiteTenderingStatus.TenderForWorksContract or SiteTenderingStatus.ContractingHasNotYetBegun);
        summary.Add(new SectionSummaryItemModel(
            "Strategic site",
            SummaryAnswerHelper.ToYesNo(site.StrategicSiteDetails.IsStrategicSite, site.StrategicSiteDetails.IsStrategicSite == true ? site.StrategicSiteDetails.StrategicSiteName : null),
            createAction(nameof(Controller.StrategicSite)),
            IsEditable: isEditable));
        summary.Add(new SectionSummaryItemModel(
            "Site type",
            SummaryAnswerHelper.ToEnum(site.SiteTypeDetails.SiteType),
            createAction(nameof(Controller.SiteType)),
            IsEditable: isEditable));
        summary.Add(new SectionSummaryItemModel(
            "Green belt",
            SummaryAnswerHelper.ToYesNo(site.SiteTypeDetails.IsOnGreenBelt),
            createAction(nameof(Controller.SiteType)),
            IsEditable: isEditable));
        summary.Add(new SectionSummaryItemModel(
            "Regeneration site",
            SummaryAnswerHelper.ToYesNo(site.SiteTypeDetails.IsRegenerationSite),
            createAction(nameof(Controller.SiteType)),
            IsEditable: isEditable));

        return summary;
    }

    private static IList<SectionSummaryItemModel> CreateSiteUseSummary(SiteModel site, CreateAction createAction, bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new(
                "Street front infill",
                SummaryAnswerHelper.ToYesNo(site.SiteUseDetails.IsPartOfStreetFrontInfill),
                createAction(nameof(Controller.SiteUse)),
                IsEditable: isEditable),
            new(
                "Traveller pitch site",
                SummaryAnswerHelper.ToYesNo(site.SiteUseDetails.IsForTravellerPitchSite),
                createAction(nameof(Controller.SiteUse)),
                IsEditable: isEditable),
        };

        summary.AddWhen(
            new(
                "Type of traveller pitch site",
                SummaryAnswerHelper.ToEnum(site.SiteUseDetails.TravellerPitchSiteType),
                createAction(nameof(Controller.TravellerPitchType)),
                IsEditable: isEditable),
            site.SiteUseDetails.IsForTravellerPitchSite == true);
        summary.Add(new(
            "Rural settlement",
            SummaryAnswerHelper.ToYesNo(site.RuralClassification.IsWithinRuralSettlement),
            createAction(nameof(Controller.RuralClassification)),
            IsEditable: isEditable));
        summary.Add(new(
            "Rural exception site",
            SummaryAnswerHelper.ToYesNo(site.RuralClassification.IsRuralExceptionSite),
            createAction(nameof(Controller.RuralClassification)),
            IsEditable: isEditable));
        summary.Add(new(
            "Actions taken to reduce environmental impact",
            site.EnvironmentalImpact.ToOneElementList(),
            createAction(nameof(Controller.EnvironmentalImpact)),
            IsEditable: isEditable));

        return summary;
    }

    private static IList<SectionSummaryItemModel> CreateMmcSummary(SiteModernMethodsOfConstruction mmc, CreateAction createAction, bool isEditable)
    {
        var summary = new List<SectionSummaryItemModel>
        {
            new("MMC", SummaryAnswerHelper.ToEnum(mmc.UsingModernMethodsOfConstruction), createAction(nameof(Controller.MmcUsing)), IsEditable: isEditable),
        };

        summary.AddWhen(
            new SectionSummaryItemModel(
                "Barriers",
                mmc.InformationBarriers.ToOneElementList(),
                createAction(nameof(Controller.MmcInformation)),
                IsEditable: isEditable),
            mmc.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.Yes or SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes);
        summary.AddWhen(
            new SectionSummaryItemModel(
                "Impact on developments",
                mmc.InformationImpact.ToOneElementList(),
                createAction(nameof(Controller.MmcInformation)),
                IsEditable: isEditable),
            mmc.UsingModernMethodsOfConstruction is SiteUsingModernMethodsOfConstruction.Yes or SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes);

        if (mmc.UsingModernMethodsOfConstruction == SiteUsingModernMethodsOfConstruction.Yes)
        {
            summary.Add(new SectionSummaryItemModel(
                "MMC categories",
                mmc.ModernMethodsConstructionCategories?.Select(x => x.GetDescription()).ToList(),
                createAction(nameof(Controller.MmcCategories)),
                IsEditable: isEditable));
            summary.AddWhen(
                new SectionSummaryItemModel(
                    "Sub-categories of 3D primary structural systems",
                    mmc.ModernMethodsConstruction3DSubcategories?.Select(x => x.GetDescription()).ToList(),
                    createAction(nameof(Controller.Mmc3DCategory)),
                    IsEditable: isEditable),
                mmc.ModernMethodsConstructionCategories?.Contains(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems) == true);
            summary.AddWhen(
                new SectionSummaryItemModel(
                    "Sub-categories of 2D primary structural systems",
                    mmc.ModernMethodsConstruction2DSubcategories?.Select(x => x.GetDescription()).ToList(),
                    createAction(nameof(Controller.Mmc2DCategory)),
                    IsEditable: isEditable),
                mmc.ModernMethodsConstructionCategories?.Contains(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems) == true);
        }

        if (mmc.UsingModernMethodsOfConstruction == SiteUsingModernMethodsOfConstruction.No)
        {
            summary.Add(new SectionSummaryItemModel(
                "Plans for adopting in the future",
                mmc.FutureAdoptionPlans.ToOneElementList(),
                createAction(nameof(Controller.MmcFutureAdoption)),
                IsEditable: isEditable));
            summary.Add(new SectionSummaryItemModel(
                "Impact",
                mmc.FutureAdoptionExpectedImpact.ToOneElementList(),
                createAction(nameof(Controller.MmcFutureAdoption)),
                IsEditable: isEditable));
        }

        return summary;
    }

    private static IList<SectionSummaryItemModel> CreateProcurementSummary(SiteModel site, CreateAction createAction, bool isEditable)
    {
        return new List<SectionSummaryItemModel>
        {
            new(
                "Procurement mechanisms",
                site.SiteProcurements.Select(x => x.GetDescription()).ToList(),
                createAction(nameof(Controller.Procurements)),
                IsEditable: isEditable),
        };
    }

    private static string CreateSiteActionUrl(IUrlHelper urlHelper, SiteId siteId, string actionName, bool useWorkflowRedirection)
    {
        var action = urlHelper.Action(
            actionName,
            new ControllerName(nameof(SiteController)).WithoutPrefix(),
            new { siteId = siteId.Value, redirect = useWorkflowRedirection ? nameof(SiteController.CheckAnswers) : null });

        return action ?? string.Empty;
    }
}
