using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
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

    private readonly BuildingForHealthyLifeType _buildingForHealthyLife = BuildingForHealthyLifeType.Yes;

    private readonly NumberOfGreenLights? _numberOfGreenLights = new("5");

    private readonly SiteUseDetails _siteUseDetails = new(true, true, TravellerPitchSiteType.Permanent);

    [Fact]
    public void ShouldReturnCheckAnswers_WhenAllDateProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            name: "site name",
            localAuthority: _localAuthority,
            planningDetails: _planningDetails,
            tenderingStatusDetails: _tenderingStatusDetails,
            section106: _section106,
            nationalDesignGuidePriorities: new List<NationalDesignGuidePriority>() { NationalDesignGuidePriority.Nature },
            buildingForHealthyLife: _buildingForHealthyLife,
            numberOfGreenLights: _numberOfGreenLights,
            landAcquisitionStatus: SiteLandAcquisitionStatus.FullOwnership,
            siteUseDetails: _siteUseDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.CheckAnswers);
    }

    [Fact]
    public void ShouldReturnName_WhenNameNotProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, localAuthority: _localAuthority, planningDetails: _planningDetails, section106: _section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
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
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnSection106AffordableHousing_WhenAffordableHousingNotProvided()
    {
        // given
        var section106 = new Section106Dto("3", "TestSite", true);

        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, section106: section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnSection106OnlyAffordableHousing_WhenOnlyAffordableHousingNotProvided()
    {
        // given
        var section106 = new Section106Dto("3", "TestSite", true, true);

        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, section106: section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnSection106AdditionalAffordableHousing_WhenAdditionalAffordableHousingNotProvided()
    {
        // given
        var section106 = new Section106Dto("3", "TestSite", true, true, false);

        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, section106: section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnSection106CapitalFundingEligibility_WhenCapitalFundingEligibilityNotProvided()
    {
        // given
        var section106 = new Section106Dto("3", "TestSite", true, true, false, true);

        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, section106: section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnSection106LocalAuthorityConfirmation_WhenLocalAuthorityConfirmationNotProvided()
    {
        // given
        var section106 = new Section106Dto("3", "TestSite", true, true, false, true, false);

        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, section106: section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnLocalAuthoritySearch_WhenLocalAuthorityNotProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, name: "some name", planningDetails: _planningDetails, section106: _section106);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthoritySearch);
    }

    [Fact]
    public void ShouldReturnPlanningStatus_WhenPlanningStatusNotProvided()
    {
        Test(SiteWorkflowState.PlanningStatus, _planningDetails with { PlanningStatus = null });
    }

    [Fact]
    public void ShouldReturnPlanningDetails_WhenPlanningDetailsNotProvided()
    {
        Test(SiteWorkflowState.PlanningDetails, _planningDetails with { ArePlanningDetailsProvided = false });
    }

    [Fact]
    public void ShouldReturnLandRegistry_WhenLandRegistryNotProvided()
    {
        Test(SiteWorkflowState.LandRegistry, _planningDetails with { LandRegistryTitleNumber = null });
    }

    [Fact]
    public void ShouldReturnNationalDesignGuide_WhenNationalDesignGuideNotProvided()
    {
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.NationalDesignGuide,
            name: "some name",
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
            name: "some name",
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
        Test(SiteWorkflowState.IntentionToWorkWithSme, tenderingStatusDetails: _tenderingStatusDetails with { TenderingStatus = SiteTenderingStatus.TenderForWorksContract, IsIntentionToWorkWithSme = null });
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

    private void Test(
        SiteWorkflowState expected,
        SitePlanningDetails? planningDetails = null,
        SiteTenderingStatusDetails? tenderingStatusDetails = null,
        BuildingForHealthyLifeType buildingForHealthyLifeType = BuildingForHealthyLifeType.NotApplicable,
        NumberOfGreenLights? numberOfGreenLights = null,
        StrategicSite? strategicSite = null,
        SiteTypeDetails? siteTypeDetails = null)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            name: "some name",
            localAuthority: _localAuthority,
            planningDetails: planningDetails ?? _planningDetails,
            tenderingStatusDetails: tenderingStatusDetails ?? _tenderingStatusDetails,
            section106: _section106,
            nationalDesignGuidePriorities: new List<NationalDesignGuidePriority> { NationalDesignGuidePriority.NoneOfTheAbove },
            buildingForHealthyLife: buildingForHealthyLifeType,
            numberOfGreenLights: numberOfGreenLights,
            landAcquisitionStatus: SiteLandAcquisitionStatus.FullOwnership,
            strategicSite: strategicSite,
            siteTypeDetails: siteTypeDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(expected);
    }
}
