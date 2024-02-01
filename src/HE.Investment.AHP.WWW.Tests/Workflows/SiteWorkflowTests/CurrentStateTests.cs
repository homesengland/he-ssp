using FluentAssertions;
using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class CurrentStateTests
{
    private readonly SitePlanningDetails _planningDetails = new(
        SitePlanningStatus.DetailedPlanningApplicationSubmitted,
        ArePlanningDetailsProvided: true,
        IsLandRegistryTitleNumberRegistered: true,
        LandRegistryTitleNumber: "LR title",
        IsGrantFundingForAllHomesCoveredByTitleNumber: false);

    [Fact]
    public void ShouldReturnCheckAnswers_WhenAllDateProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, name: "site name", planningDetails: _planningDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.CheckAnswers);
    }

    [Fact]
    public void ShouldReturnName_WhenNameNotProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, planningDetails: _planningDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
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

    private void Test(SiteWorkflowState expected, SitePlanningDetails planningDetails)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, name: "some name", planningDetails: planningDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(expected);
    }
}
