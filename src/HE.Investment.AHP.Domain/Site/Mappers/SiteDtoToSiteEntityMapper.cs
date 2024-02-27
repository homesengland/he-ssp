extern alias Org;

using System.Collections.ObjectModel;
using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Factories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using Section106Dto = HE.Common.IntegrationModel.PortalIntegrationModel.Section106Dto;
using SiteRuralClassification = HE.Investment.AHP.Domain.Site.ValueObjects.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteUseDetails;
using StrategicSiteDetails = HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite.StrategicSiteDetails;

namespace HE.Investment.AHP.Domain.Site.Mappers;

extern alias Org;

public static class SiteDtoToSiteEntityMapper
{
    private static readonly BuildingForHealthyLifeTypeMapper BuildingForHealthyLifeTypeMapper = new();
    private static readonly SiteLandAcquisitionStatusMapper SiteLandAcquisitionStatusMapper = new();
    private static readonly SiteTenderingStatusMapper SiteTenderingStatusMapper = new();
    private static readonly SiteTypeMapper SiteTypeMapper = new();
    private static readonly TravellerPitchSiteTypeMapper TravellerPitchSiteTypeMapper = new();
    private static readonly PlanningStatusMapper PlanningStatusMapper = new();
    private static readonly NationalDesignGuideMapper NationalDesignGuideMapper = new();

    public static SiteEntity Map(SiteDto dto)
    {
        // TODO: #90352 MMC mapping and others
        return new SiteEntity(
            new SiteId(dto.id),
            new SiteName(dto.name),
            SiteStatus.Completed,
            CreateSection106(dto.section106),
            CreateLocalAuthority(dto.localAuthority.id, dto.localAuthority.name),
            CreatePlanningDetails(dto.planningDetails),
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
            null,
            new SiteProcurements());
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

    private static PlanningDetails CreatePlanningDetails(PlanningDetailsDto dto)
    {
        if (dto.planningStatus == null)
        {
            return PlanningDetailsFactory.CreateEmpty();
        }

        return PlanningDetailsFactory.Create(
            PlanningStatusMapper.ToDomain(dto.planningStatus),
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
            new LandRegistryDetails(dto.isLandRegistryTitleNumber, string.IsNullOrWhiteSpace(dto.landRegistryTitleNumber) ? null : new LandRegistryTitleNumber(dto.landRegistryTitleNumber), dto.isGrantFundingForAllHomesCoveredByTitleNumber));
    }

    private static NationalDesignGuidePriorities CreateNationalDesignGuidePriorities(IList<int> nationalDesignGuidePriorities)
    {
        var priorities = nationalDesignGuidePriorities
            .Select(x => NationalDesignGuideMapper.ToDomain(x))
            .Where(x => x != null)
            .Cast<NationalDesignGuidePriority>()
            .ToList();

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
}
