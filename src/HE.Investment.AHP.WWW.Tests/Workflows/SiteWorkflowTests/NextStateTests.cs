using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(SiteWorkflowState.Start, SiteWorkflowState.Name)]
    [InlineData(SiteWorkflowState.Name, SiteWorkflowState.Section106GeneralAgreement)]
    [InlineData(SiteWorkflowState.LocalAuthoritySearch, SiteWorkflowState.LocalAuthorityResult)]
    [InlineData(SiteWorkflowState.LocalAuthorityResult, SiteWorkflowState.LocalAuthorityConfirm)]
    [InlineData(SiteWorkflowState.LocalAuthorityConfirm, SiteWorkflowState.PlanningStatus)]
    [InlineData(SiteWorkflowState.LocalAuthorityReset, SiteWorkflowState.PlanningStatus)]
    [InlineData(SiteWorkflowState.PlanningStatus, SiteWorkflowState.PlanningDetails)]
    [InlineData(SiteWorkflowState.LandRegistry, SiteWorkflowState.NationalDesignGuide)]
    [InlineData(SiteWorkflowState.NationalDesignGuide, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.NumberOfGreenLights, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.ContractorDetails, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, SiteWorkflowState.StrategicSite)]
    [InlineData(SiteWorkflowState.StrategicSite, SiteWorkflowState.SiteType)]
    [InlineData(SiteWorkflowState.SiteType, SiteWorkflowState.SiteUse)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder().Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.PlanningDetails, planningDetails: planningDetails);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.TenderingStatus, tenderingStatusDetails: details);

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
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.TenderingStatus, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.ContractorDetails, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.SiteType, SiteWorkflowState.StrategicSite)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder().Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.StrategicSite, tenderingStatusDetails: new SiteTenderingStatusDetails(tenderingStatus, null, null, null));

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106GeneralAgreement, SiteWorkflowState.Section106AffordableHousing)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AgreementTrue(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder().WithGeneralAgreement(true).Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106GeneralAgreement, SiteWorkflowState.LocalAuthoritySearch)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AgreementFalse(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder().WithGeneralAgreement(false).Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106AffordableHousing, SiteWorkflowState.Section106OnlyAffordableHousing)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AffordableHousingTrue(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
            .WithGeneralAgreement(true)
            .WithAffordableHousing(true)
            .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106AffordableHousing, SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AffordableHousingFalse(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106OnlyAffordableHousing, SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106OnlyAffordableHousingTrue(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(true)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106OnlyAffordableHousing, SiteWorkflowState.Section106AdditionalAffordableHousing)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106OnlyAffordableHousingFalse(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106AdditionalAffordableHousing, SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AdditionalAffordableHousing(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(false)
           .WithAdditionalAffordableHousing(true)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Section106Ineligible)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106CapitalFundingEligibilityTrue(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(false)
           .WithAdditionalAffordableHousing(true)
           .WithCapitalFundingEligibility(true)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Section106LocalAuthorityConfirmation, true)]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.LocalAuthoritySearch, false)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106CapitalFundingEligibilityFalse(SiteWorkflowState current, SiteWorkflowState expectedNext, bool additionalAffordableHousing)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(!additionalAffordableHousing)
           .WithAdditionalAffordableHousing(additionalAffordableHousing)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Section106OnlyAffordableHousing, true)]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Section106AdditionalAffordableHousing, false)]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Section106AffordableHousing, null)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedWithSection106CapitalFundingEligibility(SiteWorkflowState current, SiteWorkflowState expectedNext, bool? onlyAffordableHousing)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(onlyAffordableHousing)
           .WithAdditionalAffordableHousing(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedWithSection106Ineligibile()
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(false)
           .WithAdditionalAffordableHousing(false)
           .WithCapitalFundingEligibility(true)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Section106Ineligible, section106);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106CapitalFundingEligibility);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106LocalAuthorityConfirmation, SiteWorkflowState.LocalAuthoritySearch)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedWithSection106LocalAuthorityConfirmation(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(true)
           .WithAdditionalAffordableHousing(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, section106);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Fact]
    public async Task ShouldReturnSection106CapitalFundingEligibility_WhenBackTriggerExecutedWithSection106AdditionalAffordableHousingSetToFalse()
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(false)
           .WithOnlyAffordableHousing(true)
           .WithAdditionalAffordableHousing(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, section106);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106CapitalFundingEligibility);
    }

    [Fact]
    public async Task ShouldReturnSection106GeneralAgreement_WhenBackTriggerExecutedWithSection106GeneralAgreementSetToFalse()
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, section106);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106GeneralAgreement);
    }

    [Fact]
    public async Task ShouldReturnSection106LocalAuthorityConfirmation_WhenBackTriggerExecutedWithSection106AdditionalAffordableHousingSetToTrue()
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(true)
           .WithAffordableHousing(true)
           .WithOnlyAffordableHousing(false)
           .WithAdditionalAffordableHousing(true)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, section106);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106LocalAuthorityConfirmation);
    }

    [Fact]
    public async Task ShouldReturnLocalAuthoritySearch_WhenBackTriggerExecutedAndSiteDoesNotHaveAnyLocalAuthoritySelected()
    {
        // given
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.PlanningStatus, section106);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthoritySearch);
    }

    [Fact]
    public async Task ShouldReturnLocalAuthorityConfirm_WhenBackTriggerExecutedAndSiteHasLocalAuthoritySelected()
    {
        // given
        var localAuthority = new LocalAuthority() { Id = "local authority id", Name = "local authority name" };
        var section106 = new Section106TestDataBuilder()
           .WithGeneralAgreement(false)
           .WithCapitalFundingEligibility(false)
           .Build();
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.PlanningStatus, section106, localAuthority);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthorityConfirm);
    }

    [Theory]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, BuildingForHealthyLifeType.NotApplicable, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, BuildingForHealthyLifeType.No, SiteWorkflowState.LandAcquisitionStatus)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, BuildingForHealthyLifeType.Yes, SiteWorkflowState.NumberOfGreenLights)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithDifferentBuildingForHealthyLife(SiteWorkflowState current, BuildingForHealthyLifeType buildingForHealthyLifeType, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, buildingForHealthyLife: buildingForHealthyLifeType);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.NumberOfGreenLights, BuildingForHealthyLifeType.Yes, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, BuildingForHealthyLifeType.No, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, BuildingForHealthyLifeType.NotApplicable, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, BuildingForHealthyLifeType.Yes, SiteWorkflowState.NumberOfGreenLights)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedWithDifferentBuildingForHealthyLife(SiteWorkflowState current, BuildingForHealthyLifeType buildingForHealthyLifeType, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, buildingForHealthyLife: buildingForHealthyLifeType);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }
}
