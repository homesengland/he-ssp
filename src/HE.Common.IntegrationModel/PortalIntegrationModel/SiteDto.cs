using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using DataverseModel;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class SiteDto
    {
        public string id { get; set; }

        public string name { get; set; }

        public int? status { get; set; }

        public SiteLocalAuthority localAuthority { get; set; }

        public Section106Dto section106 { get; set; }

        public PlanningDetailsDto planningDetails { get; set; }

        public IList<int> nationalDesignGuidePriorities { get; set; }

        public int? buildingForHealthyLife { get; set; }

        public int? numberOfGreenLights { get; set; }

        public int? landStatus { get; set; }

        public TenderingDetailsDto tenderingDetails { get; set; }

        public StrategicSiteDetailsDto strategicSiteDetails { get; set; }

        public SiteTypeDetailsDto siteTypeDetails { get; set; }

        public SiteUseDetailsDto siteUseDetails { get; set; }

        public RuralDetailsDto ruralDetails { get; set; }

        public string environmentalImpact { get; set; }

        public ModernMethodsOfConstructionDto modernMethodsOfConstruction { get; set; }

        public IList<int> procurementMechanisms { get; set; }
        public string organizationId { get; set; }
        public string organizationName { get; set; }
        public UserAccountDto developerPartner { get; set; }
        public UserAccountDto ownerOfTheLandDuringDevelopment { get; set; }
        public UserAccountDto ownerOfTheHomesAfterCompletion { get; set; }
    }

    public class SiteLocalAuthority
    {
        public string id { get; set; }

        public string name { get; set; }
    }

    public class RuralDetailsDto
    {
        public bool? isRuralClassification { get; set; }

        public bool? isRuralExceptionSite { get; set; }
    }

    public class SiteUseDetailsDto
    {
        public bool? isPartOfStreetFrontInfill { get; set; }

        public bool? isForTravellerPitchSite { get; set; }

        public int? travellerPitchSiteType { get; set; }
    }

    public class SiteTypeDetailsDto
    {
        public int? siteType { get; set; }

        public bool? isGreenBelt { get; set; }

        public bool? isRegenerationSite { get; set; }
    }

    public class StrategicSiteDetailsDto
    {
        public bool? isStrategicSite { get; set; }

        public string name { get; set; }
    }

    public class TenderingDetailsDto
    {
        public int? tenderingStatus { get; set; }

        public string contractorName { get; set; }

        public bool? isSme { get; set; }

        public bool? isIntentionToWorkWithSme { get; set; }
    }

    public class PlanningDetailsDto
    {
        public int? planningStatus { get; set; }

        public string referenceNumber { get; set; }

        public DateTime? detailedPlanningApprovalDate { get; set; }

        public string requiredFurtherSteps { get; set; }

        public DateTime? applicationForDetailedPlanningSubmittedDate { get; set; }

        public DateTime? expectedPlanningApprovalDate { get; set; }

        public DateTime? outlinePlanningApprovalDate { get; set; }

        public DateTime? planningSubmissionDate { get; set; }

        public bool? isGrantFundingForAllHomes { get; set; }

        public bool? isLandRegistryTitleNumber { get; set; }

        public string landRegistryTitleNumber { get; set; }

        public bool? isGrantFundingForAllHomesCoveredByTitleNumber { get; set; }
    }

    public class Section106Dto
    {
        public bool? isAgreement106 { get; set; }

        public bool? areContributionsForAffordableHomes { get; set; }

        public bool? isAffordableHousing100 { get; set; }

        public bool? areAdditionalHomes { get; set; }

        public bool? areRestrictionsOrObligations { get; set; }

        public string localAuthorityConfirmation { get; set; }
    }

    public class ModernMethodsOfConstructionDto
    {
        public int? usingMmc { get; set; }

        public string mmcBarriers { get; set; }

        public string mmcImpact { get; set; }

        public IList<int> mmcCategories { get; set; }

        public IList<int> mmc3DSubcategories { get; set; }

        public IList<int> mmc2DSubcategories { get; set; }

        public string mmcFutureAdoptionPlans { get; set; }

        public string mmcFutureAdoptionExpectedImpact { get; set; }
    }
}
