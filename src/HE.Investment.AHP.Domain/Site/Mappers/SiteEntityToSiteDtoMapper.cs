extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Common.CRM.Mappers;
using Section106 = HE.Investment.AHP.Domain.Site.ValueObjects.Section106;
using Section106Dto = HE.Common.IntegrationModel.PortalIntegrationModel.Section106Dto;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public static class SiteEntityToSiteDtoMapper
{
    private static readonly BuildingForHealthyLifeTypeMapper BuildingForHealthyLifeTypeMapper = new();
    private static readonly SiteLandAcquisitionStatusMapper SiteLandAcquisitionStatusMapper = new();
    private static readonly SiteTenderingStatusMapper SiteTenderingStatusMapper = new();
    private static readonly SiteTypeMapper SiteTypeMapper = new();
    private static readonly TravellerPitchSiteTypeMapper TravellerPitchSiteTypeMapper = new();
    private static readonly NationalDesignGuideMapper NationalDesignGuideMapper = new();
    private static readonly SiteUsingModernMethodsOfConstructionMapper SiteUsingModernMethodsOfConstructionMapper = new();
    private static readonly ModernMethodsConstructionCategoriesTypeMapper ModernMethodsConstructionCategoriesTypeMapper = new();
    private static readonly ModernMethodsConstruction2DSubcategoriesTypeMapper ModernMethodsConstruction2DSubcategoriesTypeMapper = new();
    private static readonly ModernMethodsConstruction3DSubcategoriesTypeMapper ModernMethodsConstruction3DSubcategoriesTypeMapper = new();
    private static readonly SiteProcurementMapper SiteProcurementMapper = new();
    private static readonly SiteStatusMapper SiteStatusMapper = new();
    private static readonly PlanningStatusMapper PlanningStatusMapper = new();

    public static SiteDto Map(SiteEntity entity)
    {
        return new SiteDto
        {
            id = string.IsNullOrWhiteSpace(entity.Id.Value) ? null : entity.Id.Value,
            name = entity.Name.Value,
            status = SiteStatusMapper.ToDto(entity.Status),
            section106 = CreateSection106(entity.Section106),
            localAuthority = new SiteLocalAuthority { id = entity.LocalAuthority?.Code.Value, name = entity.LocalAuthority?.Name },
            planningDetails = CreatePlanningDetails(entity.PlanningDetails),
            nationalDesignGuidePriorities = MapCollection(entity.NationalDesignGuidePriorities.Values, NationalDesignGuideMapper),
            buildingForHealthyLife = BuildingForHealthyLifeTypeMapper.ToDto(entity.BuildingForHealthyLife),
            numberOfGreenLights = entity.NumberOfGreenLights?.Value,
            landStatus = entity.LandAcquisitionStatus.Value == null ? null : SiteLandAcquisitionStatusMapper.ToDto(entity.LandAcquisitionStatus.Value.Value),
            tenderingDetails = CreateTenderingDetails(entity.TenderingStatusDetails),
            strategicSiteDetails = CreateStrategicSiteDetails(entity.StrategicSiteDetails),
            siteTypeDetails = CreateSiteTypeDetails(entity.SiteTypeDetails),
            siteUseDetails = CreateSiteUseDetails(entity.SiteUseDetails),
            ruralDetails = CreateRuralDetails(entity.RuralClassification),
            environmentalImpact = entity.EnvironmentalImpact?.Value,
            modernMethodsOfConstruction = CreateModernMethodsOfConstruction(entity.ModernMethodsOfConstruction),
            procurementMechanisms = MapCollection(entity.Procurements.Procurements, SiteProcurementMapper),
        };
    }

    private static Section106Dto CreateSection106(Section106 valueObject)
    {
        return new Section106Dto
        {
            isAgreement106 = valueObject.GeneralAgreement,
            areContributionsForAffordableHomes = valueObject.AffordableHousing,
            isAffordableHousing100 = valueObject.OnlyAffordableHousing,
            areAdditionalHomes = valueObject.AdditionalAffordableHousing,
            areRestrictionsOrObligations = valueObject.CapitalFundingEligibility,
            localAuthorityConfirmation = valueObject.LocalAuthorityConfirmation,
        };
    }

    private static PlanningDetailsDto CreatePlanningDetails(PlanningDetails planningDetails)
    {
        return new PlanningDetailsDto
        {
            planningStatus = PlanningStatusMapper.ToDto(planningDetails.PlanningStatus),
            referenceNumber = planningDetails.ReferenceNumber?.Value,
            detailedPlanningApprovalDate = planningDetails.DetailedPlanningApprovalDate?.Value,
            requiredFurtherSteps = planningDetails.RequiredFurtherSteps?.Value,
            applicationForDetailedPlanningSubmittedDate = planningDetails.ApplicationForDetailedPlanningSubmittedDate?.Value,
            expectedPlanningApprovalDate = planningDetails.ExpectedPlanningApprovalDate?.Value,
            outlinePlanningApprovalDate = planningDetails.OutlinePlanningApprovalDate?.Value,
            planningSubmissionDate = planningDetails.PlanningSubmissionDate?.Value,
            isGrantFundingForAllHomes = planningDetails.IsGrantFundingForAllHomesCoveredByApplication,
            isLandRegistryTitleNumber = planningDetails.LandRegistryDetails?.IsLandRegistryTitleNumberRegistered,
            landRegistryTitleNumber = planningDetails.LandRegistryDetails?.TitleNumber?.Value,
            isGrantFundingForAllHomesCoveredByTitleNumber = planningDetails.LandRegistryDetails?.IsGrantFundingForAllHomesCoveredByTitleNumber,
        };
    }

    private static TenderingDetailsDto CreateTenderingDetails(TenderingStatusDetails details) =>
        new()
        {
            tenderingStatus = SiteTenderingStatusMapper.ToDto(details.TenderingStatus),
            contractorName = details.ContractorName?.Value,
            isSme = details.IsSmeContractor,
            isIntentionToWorkWithSme = details.IsIntentionToWorkWithSme,
        };

    private static StrategicSiteDetailsDto CreateStrategicSiteDetails(StrategicSiteDetails? details) =>
        new() { isStrategicSite = details?.IsStrategicSite, name = details?.SiteName?.Value, };

    private static SiteTypeDetailsDto CreateSiteTypeDetails(SiteTypeDetails details) =>
        new()
        {
            siteType = SiteTypeMapper.ToDto(details.SiteType),
            isGreenBelt = details.IsOnGreenBelt,
            isRegenerationSite = details.IsRegenerationSite,
        };

    private static SiteUseDetailsDto CreateSiteUseDetails(SiteUseDetails details) =>
        new()
        {
            isPartOfStreetFrontInfill = details.IsPartOfStreetFrontInfill,
            isForTravellerPitchSite = details.IsForTravellerPitchSite,
            travellerPitchSiteType = TravellerPitchSiteTypeMapper.ToDto(details.TravellerPitchSiteType),
        };

    private static RuralDetailsDto CreateRuralDetails(SiteRuralClassification details) =>
        new()
        {
            isRuralClassification = details.IsWithinRuralSettlement,
            isRuralExceptionSite = details.IsRuralExceptionSite,
        };

    private static ModernMethodsOfConstructionDto CreateModernMethodsOfConstruction(SiteModernMethodsOfConstruction mmc)
    {
        return new ModernMethodsOfConstructionDto
        {
            usingMmc = SiteUsingModernMethodsOfConstructionMapper.ToDto(mmc.SiteUsingModernMethodsOfConstruction),
            mmcBarriers = mmc.Information?.Barriers?.Value,
            mmcImpact = mmc.Information?.Impact?.Value,
            mmcCategories = MapCollection(mmc.ModernMethodsOfConstruction?.ModernMethodsConstructionCategories, ModernMethodsConstructionCategoriesTypeMapper),
            mmc3DSubcategories = MapCollection(mmc.ModernMethodsOfConstruction?.ModernMethodsConstruction3DSubcategories, ModernMethodsConstruction3DSubcategoriesTypeMapper),
            mmc2DSubcategories = MapCollection(mmc.ModernMethodsOfConstruction?.ModernMethodsConstruction2DSubcategories, ModernMethodsConstruction2DSubcategoriesTypeMapper),
            mmcFutureAdoptionPlans = mmc.FutureAdoption?.Plans?.Value,
            mmcFutureAdoptionExpectedImpact = mmc.FutureAdoption?.ExpectedImpact?.Value,
        };
    }

    private static IList<int> MapCollection<T>(IEnumerable<T>? values, EnumMapper<T> mapper)
        where T : struct
    {
        return (values ?? Enumerable.Empty<T>())
            .Select(x => mapper.ToDto(x))
            .Where(x => x != null)
            .Cast<int>()
            .ToList();
    }
}
