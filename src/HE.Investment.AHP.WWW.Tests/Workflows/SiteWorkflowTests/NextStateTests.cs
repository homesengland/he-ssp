using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(SiteWorkflowState.Start, SiteWorkflowState.Name)]
    [InlineData(SiteWorkflowState.Name, SiteWorkflowState.Section106GeneralAgreement)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, null, null, null, null, null, null);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106GeneralAgreement, SiteWorkflowState.Section106AffordableHousing)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AgreementTrue(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, true, null, null, null, null, null);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106GeneralAgreement, SiteWorkflowState.LocalAuthority)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106AgreementFalse(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, false, null, null, null, null, null);

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
        var workflow = BuildWorkflow(current, true, true, null, null, null, null);

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
        var workflow = BuildWorkflow(current, true, false, null, null, null, null);

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
        var workflow = BuildWorkflow(current, true, true, true, null, null, null);

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
        var workflow = BuildWorkflow(current, true, true, false, null, null, null);

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
        var workflow = BuildWorkflow(current, true, true, false, true, null, null);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Ineligable)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106CapitalFundingEligibilityTrue(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        // given
        var workflow = BuildWorkflow(current, true, true, false, true, true, null);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.Section106ConfirmationFromLocalAuthority, true)]
    [InlineData(SiteWorkflowState.Section106CapitalFundingEligibility, SiteWorkflowState.LocalAuthority, false)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSection106CapitalFundingEligibilityFalse(SiteWorkflowState current, SiteWorkflowState expectedNext, bool additionalAffordableHousing)
    {
        // given
        var workflow = BuildWorkflow(current, true, true, false, additionalAffordableHousing, false, null);

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
        var workflow = BuildWorkflow(current, true, true, onlyAffordableHousing, false, false, null);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    private static SiteWorkflow BuildWorkflow(
        SiteWorkflowState currentSiteWorkflowState,
        bool? section106GeneralAgreement,
        bool? section106AffordableHousing,
        bool? section106onlyAffordableHousing,
        bool? section106AdditionalAffordableHousing,
        bool? section106CapitalFundingEligibility,
        string? section106LocalAuthorityConfirmation)
    {
        var site = new SiteModel()
        {
            Section106GeneralAgreement = section106GeneralAgreement,
            Section106AffordableHousing = section106AffordableHousing,
            Section106OnlyAffordableHousing = section106onlyAffordableHousing,
            Section106AdditionalAffordableHousing = section106AdditionalAffordableHousing,
            Section106CapitalFundingEligibility = section106CapitalFundingEligibility,
            Section106ConfirmationFromLocalAuthority = section106LocalAuthorityConfirmation,
        };

        return new SiteWorkflow(currentSiteWorkflowState, site);
    }
}
