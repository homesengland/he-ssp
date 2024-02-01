using FluentAssertions;
using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class StateCanBeAccessedTests
{
    [Theory]
    [InlineData(SiteWorkflowState.Index, true)]
    [InlineData(SiteWorkflowState.Start, true)]
    [InlineData(SiteWorkflowState.Name, true)]
    [InlineData(SiteWorkflowState.LocalAuthoritySearch, true)]
    [InlineData(SiteWorkflowState.LocalAuthorityResult, true)]
    [InlineData(SiteWorkflowState.LocalAuthorityConfirm, true)]
    [InlineData(SiteWorkflowState.LocalAuthorityReset, true)]
    [InlineData(SiteWorkflowState.PlanningStatus, true)]
    [InlineData(SiteWorkflowState.PlanningDetails, true)]
    [InlineData(SiteWorkflowState.LandRegistry, false)]
    public async Task ShouldReturnValue_WhenMethodCalledForDefaults(SiteWorkflowState state, bool expectedResult)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledFor()
    {
        // given
        var planningDetails = new SitePlanningDetails(SitePlanningStatus.DetailedPlanningApplicationSubmitted, IsLandRegistryTitleNumberRegistered: true);
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, planningDetails: planningDetails);

        // when
        var result = await workflow.StateCanBeAccessed(SiteWorkflowState.LandRegistry);

        // then
        result.Should().BeTrue();
    }
}
