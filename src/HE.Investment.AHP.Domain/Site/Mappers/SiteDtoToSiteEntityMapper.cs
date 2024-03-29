extern alias Org;

using System.Collections.ObjectModel;
using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Common.CRM.Mappers;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using Section106Dto = HE.Common.IntegrationModel.PortalIntegrationModel.Section106Dto;
using SiteModernMethodsOfConstruction = HE.Investment.AHP.Domain.Site.ValueObjects.Mmc.SiteModernMethodsOfConstruction;
using SiteRuralClassification = HE.Investment.AHP.Domain.Site.ValueObjects.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteUseDetails;
using StrategicSiteDetails = HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite.StrategicSiteDetails;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public static class SiteDtoToSiteEntityMapper
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

    public static SiteEntity Map(SiteDto dto, IPlanningStatusMapper planningStatusMapper)
    {
        return new SiteEntity(
            new SiteId(dto.id),
            new SiteName(dto.name),
            SiteStatusMapper.ToDomain(dto.status),
            CreateSection106(dto.section106),
            CreateLocalAuthority(dto.localAuthority.id, dto.localAuthority.name),
            CreatePlanningDetails(dto.planningDetails, planningStatusMapper),
            CreateNationalDesignGuidePriorities(dto.nationalDesignGuidePriorities),
            BuildingForHealthyLifeTypeMapper.ToDomain(dto.buildingForHealthyLife),
            CreateNumberOfGreenLights(dto.numberOfGreenLights),
            new LandAcquisitionStatus(SiteLandAcquisitionStatusMapper.ToDomain(dto.landStatus)),
            CreateTenderingStatusDetails(dto.tenderingDetails),
            CreateStrategicSiteDetails(dto.strategicSiteDetails),
            CreateSiteTypeDetails(dto.siteTypeDetails),
            CreateSiteUseDetails(dto.siteUseDetails),
            CreateSiteRuralClassification(dto.ruralDetails),
            string.IsNullOrWhiteSpace(dto.environmentalImpact) ? null : new EnvironmentalImpact(dto.environmentalImpact),
            CreateMmc(dto.modernMethodsOfConstruction),
            new SiteProcurements(MapCollection(dto.procurementMechanisms, SiteProcurementMapper)));
    }

    private static Section106 CreateSection106(Section106Dto dto)
    {
        return dto.isAgreement106 == null
            ? new Section106()
            : new Section106(
                dto.isAgreement106,
                dto.areContributionsForAffordableHomes,
                dto.isAffordableHousing100,
                dto.areAdditionalHomes,
                dto.areRestrictionsOrObligations,
                dto.localAuthorityConfirmation);
    }

    private static Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority? CreateLocalAuthority(string id, string name)
    {
        return string.IsNullOrWhiteSpace(id)
            ? null
            : new Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority(new LocalAuthorityId(id), name);
    }

    private static PlanningDetails CreatePlanningDetails(PlanningDetailsDto dto, IPlanningStatusMapper planningStatusMapper)
    {
        if (dto.planningStatus == null)
        {
            return PlanningDetailsFactory.CreateEmpty();
        }

        return PlanningDetailsFactory.Create(
            planningStatusMapper.ToDomain(dto.planningStatus),
            string.IsNullOrWhiteSpace(dto.referenceNumber) ? null : new ReferenceNumber(dto.referenceNumber),
            CreateDate(dto.detailedPlanningApprovalDate, (day, month, year) => new DetailedPlanningApprovalDate(day, month, year)),
            string.IsNullOrWhiteSpace(dto.requiredFurtherSteps) ? null : new RequiredFurtherSteps(dto.requiredFurtherSteps),
            CreateDate(
                dto.applicationForDetailedPlanningSubmittedDate,
                (day, month, year) => new ApplicationForDetailedPlanningSubmittedDate(day, month, year)),
            CreateDate(dto.expectedPlanningApprovalDate, (day, month, year) => new ExpectedPlanningApprovalDate(day, month, year)),
            CreateDate(dto.outlinePlanningApprovalDate, (day, month, year) => new OutlinePlanningApprovalDate(day, month, year)),
            CreateDate(dto.planningSubmissionDate, (day, month, year) => new PlanningSubmissionDate(day, month, year)),
            dto.isGrantFundingForAllHomes,
            new LandRegistryDetails(
                dto.isLandRegistryTitleNumber,
                string.IsNullOrWhiteSpace(dto.landRegistryTitleNumber) ? null : new LandRegistryTitleNumber(dto.landRegistryTitleNumber),
                dto.isGrantFundingForAllHomesCoveredByTitleNumber));
    }

    private static NationalDesignGuidePriorities CreateNationalDesignGuidePriorities(IList<int> nationalDesignGuidePriorities)
    {
        var priorities = MapCollection(nationalDesignGuidePriorities, NationalDesignGuideMapper);

        return priorities.Any()
            ? new NationalDesignGuidePriorities(new ReadOnlyCollection<NationalDesignGuidePriority>(priorities))
            : new NationalDesignGuidePriorities();
    }

    private static NumberOfGreenLights? CreateNumberOfGreenLights(int? dto)
    {
        return dto != null ? new NumberOfGreenLights(dto.Value.ToString(CultureInfo.InvariantCulture)) : null;
    }

    private static TenderingStatusDetails CreateTenderingStatusDetails(TenderingDetailsDto dto)
    {
        return TenderingStatusDetails.Create(
            SiteTenderingStatusMapper.ToDomain(dto.tenderingStatus),
            string.IsNullOrWhiteSpace(dto.contractorName) ? null : new ContractorName(dto.contractorName),
            dto.isSme,
            dto.isIntentionToWorkWithSme);
    }

    private static StrategicSiteDetails CreateStrategicSiteDetails(StrategicSiteDetailsDto dto)
    {
        return new StrategicSiteDetails(dto.isStrategicSite, string.IsNullOrEmpty(dto.name) ? null : new StrategicSiteName(dto.name));
    }

    private static SiteTypeDetails CreateSiteTypeDetails(SiteTypeDetailsDto dto)
    {
        return new SiteTypeDetails(SiteTypeMapper.ToDomain(dto.siteType), dto.isGreenBelt, dto.isRegenerationSite);
    }

    private static SiteUseDetails CreateSiteUseDetails(SiteUseDetailsDto dto)
    {
        return new SiteUseDetails(
            dto.isPartOfStreetFrontInfill,
            dto.isForTravellerPitchSite,
            TravellerPitchSiteTypeMapper.ToDomain(dto.travellerPitchSiteType));
    }

    private static SiteRuralClassification CreateSiteRuralClassification(RuralDetailsDto dto) =>
        new(dto.isRuralClassification, dto.isRuralExceptionSite);

    private static SiteModernMethodsOfConstruction CreateMmc(ModernMethodsOfConstructionDto dto) =>
        new(
            SiteUsingModernMethodsOfConstructionMapper.ToDomain(dto.usingMmc),
            new ModernMethodsOfConstructionInformation(
                string.IsNullOrWhiteSpace(dto.mmcBarriers) ? null : new ModernMethodsOfConstructionBarriers(dto.mmcBarriers),
                string.IsNullOrWhiteSpace(dto.mmcImpact) ? null : new ModernMethodsOfConstructionImpact(dto.mmcImpact)),
            new ModernMethodsOfConstructionFutureAdoption(
                string.IsNullOrWhiteSpace(dto.mmcFutureAdoptionPlans) ? null : new ModernMethodsOfConstructionPlans(dto.mmcFutureAdoptionPlans),
                string.IsNullOrWhiteSpace(dto.mmcFutureAdoptionExpectedImpact) ? null : new ModernMethodsOfConstructionExpectedImpact(dto.mmcFutureAdoptionExpectedImpact)),
            new ModernMethodsOfConstruction(
                MapCollection(dto.mmcCategories, ModernMethodsConstructionCategoriesTypeMapper),
                MapCollection(dto.mmc2DSubcategories, ModernMethodsConstruction2DSubcategoriesTypeMapper),
                MapCollection(dto.mmc3DSubcategories, ModernMethodsConstruction3DSubcategoriesTypeMapper)));

    private static T? CreateDate<T>(DateTime? date, Func<string, string, string, T?> create)
        where T : class
    {
        return date == null
            ? null
            : create(
                date.Value.Day.ToString(CultureInfo.InvariantCulture),
                date.Value.Month.ToString(CultureInfo.InvariantCulture),
                date.Value.Year.ToString(CultureInfo.InvariantCulture));
    }

    private static IList<T> MapCollection<T>(IList<int>? values, EnumMapper<T> mapper)
        where T : struct
    {
        return (values ?? Enumerable.Empty<int>())
            .Select(x => mapper.ToDomain(x))
            .Where(x => x != null)
            .Cast<T>()
            .ToList();
    }
}
