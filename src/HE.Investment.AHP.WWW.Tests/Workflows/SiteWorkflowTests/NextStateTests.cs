using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(SiteWorkflowState.Name, SiteWorkflowState.Section106GeneralAgreement)]
    [InlineData(SiteWorkflowState.LocalAuthoritySearch, SiteWorkflowState.LocalAuthorityResult)]
    [InlineData(SiteWorkflowState.LocalAuthorityResult, SiteWorkflowState.LocalAuthorityConfirm)]
    [InlineData(SiteWorkflowState.LocalAuthorityConfirm, SiteWorkflowState.PlanningStatus)]
    [InlineData(SiteWorkflowState.LocalAuthorityReset, SiteWorkflowState.PlanningStatus)]
    [InlineData(SiteWorkflowState.PlanningStatus, SiteWorkflowState.PlanningDetails)]
    [InlineData(SiteWorkflowState.LandRegistry, SiteWorkflowState.NationalDesignGuide)]
    [InlineData(SiteWorkflowState.NationalDesignGuide, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, SiteWorkflowState.StartSitePartnersFlow)]
    [InlineData(SiteWorkflowState.NumberOfGreenLights, SiteWorkflowState.StartSitePartnersFlow)]
    [InlineData(SiteWorkflowState.FinishSitePartnersFlow, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.ContractorDetails, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteWorkflowState.StrategicSite, SiteWorkflowState.SiteType)]
    [InlineData(SiteWorkflowState.SiteType, SiteWorkflowState.SiteUse)]
    [InlineData(SiteWorkflowState.SiteUse, SiteWorkflowState.RuralClassification)]
    [InlineData(SiteWorkflowState.RuralClassification, SiteWorkflowState.EnvironmentalImpact)]
    [InlineData(SiteWorkflowState.EnvironmentalImpact, SiteWorkflowState.MmcUsing)]
    [InlineData(SiteWorkflowState.MmcUsing, SiteWorkflowState.Procurements)]
    [InlineData(SiteWorkflowState.Procurements, SiteWorkflowState.CheckAnswers)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder().Build();
        var workflow = new SiteWorkflow(current, SiteModelBuilder.Build(section106));

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(null, SiteWorkflowState.NationalDesignGuide)]
    [InlineData(true, SiteWorkflowState.LandRegistry)]
    [InlineData(false, SiteWorkflowState.NationalDesignGuide)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForPlanningDetails(bool? isLandRegistryTitleNumberRegistered, SiteWorkflowState expectedState)
    {
        // given
        var planningDetails = new SitePlanningDetails(SitePlanningStatus.DetailedPlanningApplicationSubmitted, IsLandRegistryTitleNumberRegistered: isLandRegistryTitleNumberRegistered);
        var workflow = new SiteWorkflow(SiteWorkflowState.PlanningDetails, SiteModelBuilder.Build(planningDetails: planningDetails));

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedState);
    }

    [Theory]
    [InlineData(null, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteTenderingStatus.NotApplicable, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteTenderingStatus.UnconditionalWorksContract, SiteWorkflowState.ContractorDetails)]
    [InlineData(SiteTenderingStatus.ConditionalWorksContract, SiteWorkflowState.ContractorDetails)]
    [InlineData(SiteTenderingStatus.TenderForWorksContract, SiteWorkflowState.IntentionToWorkWithSme)]
    [InlineData(SiteTenderingStatus.ContractingHasNotYetBegun, SiteWorkflowState.IntentionToWorkWithSme)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForTenderingStatus(SiteTenderingStatus? tenderingStatus, SiteWorkflowState expectedState)
    {
        // given
        var details = new SiteTenderingStatusDetails(tenderingStatus, null, null, null);
        var workflow = new SiteWorkflow(SiteWorkflowState.TenderingStatus, SiteModelBuilder.Build(tenderingStatusDetails: details));

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedState);
    }

    [Theory]
    [InlineData(SiteWorkflowState.LocalAuthorityResult, SiteWorkflowState.LocalAuthoritySearch)]
    [InlineData(SiteWorkflowState.LocalAuthorityReset, SiteWorkflowState.LocalAuthoritySearch)]
    [InlineData(SiteWorkflowState.PlanningDetails, SiteWorkflowState.PlanningStatus)]
    [InlineData(SiteWorkflowState.LandRegistry, SiteWorkflowState.PlanningDetails)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, SiteWorkflowState.NationalDesignGuide)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, SiteWorkflowState.FinishSitePartnersFlow)]
    [InlineData(SiteWorkflowState.StartSitePartnersFlow, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.TenderingStatus, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.ContractorDetails, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.SiteType, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteWorkflowState.SiteUse, SiteWorkflowState.SiteType)]
    [InlineData(SiteWorkflowState.RuralClassification, SiteWorkflowState.SiteUse)]
    [InlineData(SiteWorkflowState.EnvironmentalImpact, SiteWorkflowState.RuralClassification)]
    [InlineData(SiteWorkflowState.MmcUsing, SiteWorkflowState.EnvironmentalImpact)]
    [InlineData(SiteWorkflowState.Procurements, SiteWorkflowState.MmcUsing)]
    [InlineData(SiteWorkflowState.CheckAnswers, SiteWorkflowState.Procurements)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder().Build();
        var workflow = new SiteWorkflow(current, SiteModelBuilder.Build(section106));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(null, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteTenderingStatus.NotApplicable, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteTenderingStatus.UnconditionalWorksContract, SiteWorkflowState.ContractorDetails)]
    [InlineData(SiteTenderingStatus.ConditionalWorksContract, SiteWorkflowState.ContractorDetails)]
    [InlineData(SiteTenderingStatus.TenderForWorksContract, SiteWorkflowState.IntentionToWorkWithSme)]
    [InlineData(SiteTenderingStatus.ContractingHasNotYetBegun, SiteWorkflowState.IntentionToWorkWithSme)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedForStrategicSite(SiteTenderingStatus? tenderingStatus, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = new SiteWorkflow(SiteWorkflowState.StrategicSite, SiteModelBuilder.Build(tenderingStatusDetails: new SiteTenderingStatusDetails(tenderingStatus, null, null, null)));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Fact]
    public async Task ShouldReturnLocalAuthoritySearch_WhenBackTriggerExecutedAndSiteDoesNotHaveAnyLocalAuthoritySelected()
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = new SiteWorkflow(SiteWorkflowState.PlanningStatus, SiteModelBuilder.Build(section106));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthoritySearch);
    }

    [Fact]
    public async Task ShouldReturnLocalAuthorityConfirm_WhenBackTriggerExecutedAndSiteHasLocalAuthoritySelected()
    {
        // given
        var localAuthority = new LocalAuthority("local authority id", "local authority name");
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = new SiteWorkflow(SiteWorkflowState.PlanningStatus, SiteModelBuilder.Build(section106, localAuthority));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthorityConfirm);
    }

    [Theory]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, BuildingForHealthyLifeType.NotApplicable, SiteWorkflowState.StartSitePartnersFlow)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, BuildingForHealthyLifeType.No, SiteWorkflowState.StartSitePartnersFlow)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, BuildingForHealthyLifeType.Yes, SiteWorkflowState.NumberOfGreenLights)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithDifferentBuildingForHealthyLife(SiteWorkflowState current, BuildingForHealthyLifeType buildingForHealthyLifeType, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = new SiteWorkflow(current, SiteModelBuilder.Build(buildingForHealthyLife: buildingForHealthyLifeType));

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.NumberOfGreenLights, BuildingForHealthyLifeType.Yes, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.StartSitePartnersFlow, BuildingForHealthyLifeType.No, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.StartSitePartnersFlow, BuildingForHealthyLifeType.NotApplicable, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.StartSitePartnersFlow, BuildingForHealthyLifeType.Yes, SiteWorkflowState.NumberOfGreenLights)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedWithDifferentBuildingForHealthyLife(SiteWorkflowState current, BuildingForHealthyLifeType buildingForHealthyLifeType, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = new SiteWorkflow(current, SiteModelBuilder.Build(buildingForHealthyLife: buildingForHealthyLifeType));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(true, SiteWorkflowState.TravellerPitchType)]
    [InlineData(false, SiteWorkflowState.RuralClassification)]
    [InlineData(null, SiteWorkflowState.RuralClassification)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithDifferentSiteUse(bool? isForTravellerPitchSite, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = new SiteWorkflow(
            SiteWorkflowState.SiteUse,
            SiteModelBuilder.Build(siteUseDetails: new SiteUseDetails(true, isForTravellerPitchSite, TravellerPitchSiteType.Undefined)));

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Fact]
    public async Task ShouldReturnLocalAuthorityConfirmPage_WhenBackTriggeredForPlanningStatusAndLocalAuthorityIsProvided()
    {
        // given
        var localAuthority = new LocalAuthority("local authority id", "local authority name");
        var workflow = new SiteWorkflow(SiteWorkflowState.PlanningStatus, SiteModelBuilder.Build(localAuthority: localAuthority));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthorityConfirm);
    }
}
