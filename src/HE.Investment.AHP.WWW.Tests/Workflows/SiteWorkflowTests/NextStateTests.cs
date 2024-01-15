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
            Section106AffordableHomes = section106AffordableHousing,
            Section106OnlyAffordableHomes = section106onlyAffordableHousing,
            Section106AdditionalAffordableHomes = section106AdditionalAffordableHousing,
            Section106CapitalFundingEligibility = section106CapitalFundingEligibility,
            Section106ConfirmationFromLocalAuthority = section106LocalAuthorityConfirmation,
        };

        return new SiteWorkflow(currentSiteWorkflowState, site);
    }
}
