using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.WWW.Workflows;
using SiteRuralClassification = HE.Investment.AHP.Contract.Site.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Contract.Site.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Contract.Site.SiteUseDetails;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class CurrentStateTests
{
    private readonly SitePlanningDetails _planningDetails = new(
        SitePlanningStatus.DetailedPlanningApplicationSubmitted,
        ArePlanningDetailsProvided: true,
        IsLandRegistryTitleNumberRegistered: true,
        LandRegistryTitleNumber: "LR title",
        IsGrantFundingForAllHomesCoveredByTitleNumber: false);

    private readonly SiteTenderingStatusDetails _tenderingStatusDetails = new(SiteTenderingStatus.ConditionalWorksContract, "traktor name", true, null);

    private readonly LocalAuthority _localAuthority = new() { Id = "1", Name = "Liverpool" };

    private readonly Section106Dto _section106 = new("3", "TestSite", false);

    private readonly SiteModernMethodsOfConstruction _modernMethodsOfConstruction = new(
        SiteUsingModernMethodsOfConstruction.Yes,
        new List<ModernMethodsConstructionCategoriesType>
        {
            ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
            ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
            ModernMethodsConstructionCategoriesType.Category5PreManufacturingNonStructuralAssembliesAndSubAssemblies,
        },
        new List<ModernMethodsConstruction2DSubcategoriesType> { ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation, },
        new List<ModernMethodsConstruction3DSubcategoriesType> { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut, },
        null,
        null,
        "barriers",
        "impact");

    private EnvironmentalImpact? _environmentalImpact = new("reducing environmental impact");

    [Fact]
    public void ShouldReturnCheckAnswers_WhenAllDataProvided()
    {
        Test(SiteWorkflowState.CheckAnswers);
    }

    [Fact]
    public void ShouldReturnStart_WhenSiteDoesNotExist()
    {
        // given
        var workflow = new SiteWorkflow(SiteWorkflowState.Start, null);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Start);
    }

    [Fact]
    public void ShouldReturnSection106GeneralAgreement_WhenGeneralAgreementNotProvided()
    {
        // given
        var section106 = new Section106Dto("3", "TestSite", null);

        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, section106: section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Section106GeneralAgreement);
    }

    [Fact]
    public void ShouldReturnLocalAuthoritySearch_WhenLocalAuthorityNotProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            planningDetails: _planningDetails,
            section106: _section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthoritySearch);
    }

    [Fact]
    public void ShouldReturnPlanningStatus_WhenPlanningStatusNotProvided()
    {
        Test(SiteWorkflowState.PlanningStatus, planningDetails: _planningDetails with { PlanningStatus = null });
    }

    [Fact]
    public void ShouldReturnPlanningDetails_WhenPlanningDetailsNotProvided()
    {
        Test(SiteWorkflowState.PlanningDetails, planningDetails: _planningDetails with { ArePlanningDetailsProvided = false });
    }

    [Fact]
    public void ShouldReturnLandRegistry_WhenLandRegistryNotProvided()
    {
        Test(SiteWorkflowState.LandRegistry, planningDetails: _planningDetails with { LandRegistryTitleNumber = null });
    }

    [Fact]
    public void ShouldReturnNationalDesignGuide_WhenNationalDesignGuideNotProvided()
    {
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.NationalDesignGuide,
            planningDetails: _planningDetails,
            section106: _section106,
            localAuthority: _localAuthority,
            nationalDesignGuidePriorities: null);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.NationalDesignGuide);
    }

    [Fact]
    public void ShouldReturnLandAcquisitionStatus_WhenLandAcquisitionStatusNotProvided()
    {
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.NationalDesignGuide,
            planningDetails: _planningDetails,
            section106: _section106,
            localAuthority: _localAuthority,
            nationalDesignGuidePriorities: new List<NationalDesignGuidePriority>() { NationalDesignGuidePriority.Nature },
            buildingForHealthyLife: BuildingForHealthyLifeType.No);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.LandAcquisitionStatus);
    }

    [Fact]
    public void ShouldReturnTenderingStatus_WhenTenderingStatusNotProvided()
    {
        Test(SiteWorkflowState.TenderingStatus, tenderingStatusDetails: _tenderingStatusDetails with { TenderingStatus = null });
    }

    [Fact]
    public void ShouldReturnContractorDetails_WhenContractorNameNotProvided()
    {
        Test(SiteWorkflowState.ContractorDetails, tenderingStatusDetails: _tenderingStatusDetails with { ContractorName = null });
    }

    [Fact]
    public void ShouldReturnContractorDetails_WhenIsSmeContractorNotProvided()
    {
        Test(SiteWorkflowState.ContractorDetails, tenderingStatusDetails: _tenderingStatusDetails with { IsSmeContractor = null });
    }

    [Fact]
    public void ShouldReturnIntentionToWorkWithSme_WhenIsIntentionToWorkWithSmeNotProvided()
    {
        Test(
            SiteWorkflowState.IntentionToWorkWithSme,
            tenderingStatusDetails: _tenderingStatusDetails with
            {
                TenderingStatus = SiteTenderingStatus.TenderForWorksContract,
                IsIntentionToWorkWithSme = null,
            });
    }

    [Fact]
    public void ShouldReturnBuildingForHealthyLife_WhenBuildingForHealthyLifeNotProvided()
    {
        Test(SiteWorkflowState.BuildingForHealthyLife, buildingForHealthyLifeType: BuildingForHealthyLifeType.Undefined);
    }

    [Fact]
    public void ShouldReturnNumberOfGreenLights_WhenBuildingForHealthyLifeIsYesAndNumberOfGreenLightsIsNotProvided()
    {
        Test(SiteWorkflowState.NumberOfGreenLights, buildingForHealthyLifeType: BuildingForHealthyLifeType.Yes);
    }

    [Fact]
    public void ShouldReturnStrategicSite_WhenStrategicSiteNotProvided()
    {
        Test(SiteWorkflowState.StrategicSite, strategicSite: new StrategicSite(null, null));
    }

    [Fact]
    public void ShouldReturnSiteType_WhenSiteTypeNotProvided()
    {
        Test(SiteWorkflowState.SiteType, siteTypeDetails: new SiteTypeDetails(null, null, null, false));
    }

    [Fact]
    public void ShouldReturnSiteUse_WhenSiteUseNotProvided()
    {
        Test(SiteWorkflowState.SiteUse, siteUseDetails: new SiteUseDetails(true, null, TravellerPitchSiteType.Undefined));
    }

    [Fact]
    public void ShouldReturnTravellerPitchType_WhenTravellerPitchTypeNotProvidedAndItIsUsed()
    {
        Test(SiteWorkflowState.TravellerPitchType, siteUseDetails: new SiteUseDetails(false, true, TravellerPitchSiteType.Undefined));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, true)]
    [InlineData(false, null)]
    public void ShouldReturnRuralClassification_WhenRuralClassificationNotProvided(bool? isWithinRuralSettlement, bool? isRuralExceptionSite)
    {
        Test(SiteWorkflowState.RuralClassification, ruralClassification: new SiteRuralClassification(isWithinRuralSettlement, isRuralExceptionSite));
    }

    [Fact]
    public void ShouldReturnEnvironmentalImpact_WhenEnvironmentalImpactIsNotProvided()
    {
        _environmentalImpact = null;
        Test(SiteWorkflowState.EnvironmentalImpact);
    }

    [Fact]
    public void ShouldReturnMmcUsing_WhenUsingModernMethodsOfConstructionNotProvided()
    {
        Test(SiteWorkflowState.MmcUsing, modernMethodsOfConstruction: _modernMethodsOfConstruction with { UsingModernMethodsOfConstruction = null });
    }

    [Fact]
    public void ShouldReturnMmcInformation_WhenInformationImpactNotProvided()
    {
        Test(SiteWorkflowState.MmcInformation, modernMethodsOfConstruction: _modernMethodsOfConstruction with { InformationImpact = null });
    }

    [Fact]
    public void ShouldReturnMmcInformation_WhenInformationBarriersNotProvided()
    {
        Test(SiteWorkflowState.MmcInformation, modernMethodsOfConstruction: _modernMethodsOfConstruction with { InformationBarriers = null });
    }

    [Fact]
    public void ShouldReturnMmcCategories_WhenModernMethodsConstructionCategoriesNotProvided()
    {
        Test(SiteWorkflowState.MmcCategories, modernMethodsOfConstruction: _modernMethodsOfConstruction with { ModernMethodsConstructionCategories = null });
    }

    [Fact]
    public void ShouldReturnMmcCategories_WhenModernMethodsConstructionCategoriesEmpty()
    {
        Test(SiteWorkflowState.MmcCategories, modernMethodsOfConstruction: _modernMethodsOfConstruction with { ModernMethodsConstructionCategories = new List<ModernMethodsConstructionCategoriesType>() });
    }

    [Fact]
    public void ShouldReturnMmc3DCategory_WhenModernMethodsConstruction3DSubcategoriesNotProvided()
    {
        Test(SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction: _modernMethodsOfConstruction with { ModernMethodsConstruction3DSubcategories = null });
    }

    [Fact]
    public void ShouldReturnMmc3DCategory_WhenModernMethodsConstruction3DSubcategoriesEmpty()
    {
        Test(SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction: _modernMethodsOfConstruction with { ModernMethodsConstruction3DSubcategories = new List<ModernMethodsConstruction3DSubcategoriesType>() });
    }

    [Fact]
    public void ShouldReturnMmcMmc2DCategory_WhenModernMethodsConstruction2DSubcategoriesNotProvided()
    {
        Test(SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction: _modernMethodsOfConstruction with { ModernMethodsConstruction2DSubcategories = null });
    }

    [Fact]
    public void ShouldReturnMmcMmc2DCategory_WhenModernMethodsConstruction2DSubcategoriesEmpty()
    {
        Test(SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction: _modernMethodsOfConstruction with { ModernMethodsConstruction2DSubcategories = new List<ModernMethodsConstruction2DSubcategoriesType>() });
    }

    [Fact]
    public void ShouldReturnProcurement_WhenProcurementNotProvided()
    {
        Test(SiteWorkflowState.Procurements, procurements: new List<SiteProcurement>());
    }

    private void Test(
        SiteWorkflowState expected,
        SitePlanningDetails? planningDetails = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null,
        BuildingForHealthyLifeType buildingForHealthyLifeType = BuildingForHealthyLifeType.NotApplicable,
        NumberOfGreenLights? numberOfGreenLights = null,
        StrategicSite? strategicSite = null,
        SiteTypeDetails? siteTypeDetails = null,
        SiteUseDetails? siteUseDetails = null,
        IList<SiteProcurement>? procurements = null,
        SiteRuralClassification? ruralClassification = null,
        EnvironmentalImpact? environmentalImpact = null,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            localAuthority: _localAuthority,
            planningDetails: planningDetails ?? _planningDetails,
            tenderingStatusDetails: tenderingStatusDetails ?? _tenderingStatusDetails,
            section106: _section106,
            nationalDesignGuidePriorities: new List<NationalDesignGuidePriority> { NationalDesignGuidePriority.NoneOfTheAbove },
            buildingForHealthyLife: buildingForHealthyLifeType,
            numberOfGreenLights: numberOfGreenLights,
            landAcquisitionStatus: SiteLandAcquisitionStatus.FullOwnership,
            strategicSite: strategicSite,
            siteTypeDetails: siteTypeDetails,
            siteUseDetails: siteUseDetails ?? new SiteUseDetails(false, true, TravellerPitchSiteType.Permanent),
            procurements: procurements ?? new List<SiteProcurement> { SiteProcurement.PartneringArrangementsWithContractor, SiteProcurement.LargeScaleContractProcurementThroughConsortium, },
            ruralClassification: ruralClassification ?? new SiteRuralClassification(true, false),
            environmentalImpact: environmentalImpact ?? _environmentalImpact,
            modernMethodsOfConstruction: modernMethodsOfConstruction ?? _modernMethodsOfConstruction);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(expected);
    }
}
