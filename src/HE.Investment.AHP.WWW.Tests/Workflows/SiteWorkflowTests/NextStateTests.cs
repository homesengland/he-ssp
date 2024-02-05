using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
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
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.ContractorDetails, SiteWorkflowState.CheckAnswers)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, SiteWorkflowState.CheckAnswers)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, null, null, null, null, null, null, false);

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
    [InlineData(null, SiteWorkflowState.CheckAnswers)]
    [InlineData(SiteTenderingStatus.NotApplicable, SiteWorkflowState.CheckAnswers)]
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
    [InlineData(SiteWorkflowState.TenderingStatus, SiteWorkflowState.BuildingForHealthyLife)]
    [InlineData(SiteWorkflowState.ContractorDetails, SiteWorkflowState.TenderingStatus)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, SiteWorkflowState.TenderingStatus)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, null, null, null, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, null, null, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, false, null, null, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, null, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, false, null, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, true, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, false, null, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, false, true, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, false, true, true, null, true);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, false, additionalAffordableHousing, false, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, onlyAffordableHousing, false, false, null, false);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106LocalAuthorityConfirmation, SiteWorkflowState.LocalAuthoritySearch)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedWithSection106LocalAuthorityConfirmation(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(current, true, true, true, false, false, null, false);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Fact]
    public async Task ShouldReturnSection106CapitalFundingEligibility_WhenBackTriggerExecutedWithSection106AdditionalAffordableHousingSetToFalse()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, null, null, null, false, null, null, false);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106CapitalFundingEligibility);
    }

    [Fact]
    public async Task ShouldReturnSection106GeneralAgreement_WhenBackTriggerExecutedWithSection106GeneralAgreementSetToFalse()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, false, null, null, false, null, null, false);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106GeneralAgreement);
    }

    [Fact]
    public async Task ShouldReturnSection106LocalAuthorityConfirmation_WhenBackTriggerExecutedWithSection106AdditionalAffordableHousingSetToTrue()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.LocalAuthoritySearch, true, true, false, true, null, null, false);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.Section106LocalAuthorityConfirmation);
    }

    [Fact]
    public async Task ShouldReturnLocalAuthoritySearch_WhenBackTriggerExecutedAndSiteDoesNotHaveAnyLocalAuthoritySelected()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.PlanningStatus, true, true, false, true, null, null, false);

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
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.PlanningStatus, true, true, false, true, null, null, false, localAuthority);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(SiteWorkflowState.LocalAuthorityConfirm);
    }
}
