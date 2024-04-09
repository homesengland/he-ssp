using System.Globalization;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.FillSite.Data;

public class SiteData
{
    public SiteData()
    {
    }

    public string SiteId { get; private set; }

    public string SiteName { get; private set; }

    public string LocalAuthorityId => "E08000012";

    public string LocalAuthorityName => "Liverpool";

    public YesNoType Section106GeneralAgreement => YesNoType.Yes;

    public YesNoType Section106AffordableHousing => YesNoType.Yes;

    public YesNoType Section106OnlyAffordableHousing => YesNoType.No;

    public YesNoType Section106AdditionalAffordableHousing => YesNoType.Yes;

    public YesNoType Section106CapitalFundingEligibility => YesNoType.No;

    public string LocalAuthorityConfirmation { get; private set; }

    public SitePlanningStatus PlanningStatus => SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice;

    public DateDetails ExpectedPlanningApprovalDate => new("1", "12", "2024");

    public YesNoType IsLandRegistryTitleNumberRegistered => YesNoType.Yes;

    public string LandRegistryTitleNumber { get; private set; }

    public YesNoType IsGrantFundingForAllHomesCoveredByTitleNumber => YesNoType.Yes;

    public IReadOnlyCollection<NationalDesignGuidePriority> NationalDesignGuidePriorities => new[]
    {
        NationalDesignGuidePriority.Nature,
        NationalDesignGuidePriority.Movement,
        NationalDesignGuidePriority.Resources,
    };

    public BuildingForHealthyLifeType BuildingForHealthyLife => BuildingForHealthyLifeType.Yes;

    public string NumberOfGreenLights => "5";

    public SiteLandAcquisitionStatus LandAcquisitionStatus => SiteLandAcquisitionStatus.ConditionalAcquisition;

    public SiteTenderingStatus TenderingStatus => SiteTenderingStatus.ConditionalWorksContract;

    public string ContractorName { get; private set; }

    public YesNoType IsSmeContractor => YesNoType.No;

    public YesNoType IsStrategicSite => YesNoType.Yes;

    public string StrategicSiteName { get; private set; }

    public SiteType SiteType => SiteType.Greenfield;

    public YesNoType IsOnGreenBelt => YesNoType.Yes;

    public YesNoType IsRegenerationSite => YesNoType.No;

    public YesNoType IsPartOfStreetFrontInfill => YesNoType.No;

    public YesNoType IsForTravellerPitchSite => YesNoType.Yes;

    public TravellerPitchSiteType TravellerPitchSiteType => TravellerPitchSiteType.Permanent;

    public YesNoType IsWithinRuralSettlement => YesNoType.Yes;

    public YesNoType IsRuralExceptionSite => YesNoType.Yes;

    public string EnvironmentalImpact { get; private set; }

    public SiteUsingModernMethodsOfConstruction UsingMmc { get; private set; } = SiteUsingModernMethodsOfConstruction.Yes;

    public string InformationBarriers { get; private set; }

    public string InformationImpact { get; private set; }

    public IReadOnlyCollection<ModernMethodsConstructionCategoriesType> MmcCategories { get; private set; } = new[]
    {
        ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
        ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
        ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
    };

    public ModernMethodsConstruction3DSubcategoriesType Mmc3DSubcategory { get; private set; } = ModernMethodsConstruction3DSubcategoriesType.StructuralChassisOnly;

    public ModernMethodsConstruction2DSubcategoriesType Mmc2DSubcategory { get; private set; } = ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation;

    public IReadOnlyCollection<SiteProcurement> Procurements => new[] { SiteProcurement.BulkPurchaseOfComponents, SiteProcurement.PartneringSupplyChain };

    public string GenerateSiteName()
    {
        SiteName = $"IT_Site_{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
        return SiteName;
    }

    public string GenerateLocalAuthorityConfirmation() => LocalAuthorityConfirmation = nameof(LocalAuthorityConfirmation).WithTimestampPrefix();

    public string GenerateLandRegistryTitleNumber() => LandRegistryTitleNumber = nameof(LandRegistryTitleNumber).WithTimestampPrefix();

    public string GenerateContractorName() => ContractorName = nameof(ContractorName).WithTimestampPrefix();

    public string GenerateStrategicSiteName() => StrategicSiteName = nameof(StrategicSiteName).WithTimestampPrefix();

    public string GenerateEnvironmentalImpact() => EnvironmentalImpact = nameof(EnvironmentalImpact).WithTimestampPrefix();

    public string GenerateInformationBarriers() => InformationBarriers = nameof(InformationBarriers).WithTimestampPrefix();

    public string GenerateInformationImpact() => InformationImpact = nameof(InformationImpact).WithTimestampPrefix();

    public SiteUsingModernMethodsOfConstruction ChangeMmcUsingAnswer()
    {
        UsingMmc = SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes;
        MmcCategories = new List<ModernMethodsConstructionCategoriesType>();
        Mmc2DSubcategory = ModernMethodsConstruction2DSubcategoriesType.Undefined;
        Mmc3DSubcategory = ModernMethodsConstruction3DSubcategoriesType.Undefined;
        return UsingMmc;
    }

    public void SetSiteId(string siteId)
    {
        SiteId = siteId;
    }
}
