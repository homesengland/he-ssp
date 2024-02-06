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

    private readonly SiteTenderingStatusDetails _tenderingStatusDetails = new(SiteTenderingStatus.ConditionalWorksContract, "traktor name", true, null);

    private readonly LocalAuthority _localAuthority = new() { Id = "1", Name = "Liverpool" };

    [Fact]
    public void ShouldReturnCheckAnswers_WhenAllDateProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            name: "site name",
            localAuthority: _localAuthority,
            planningDetails: _planningDetails,
            tenderingStatusDetails: _tenderingStatusDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.CheckAnswers);
    }

    [Fact]
    public void ShouldReturnName_WhenNameNotProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, localAuthority: _localAuthority, planningDetails: _planningDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(SiteWorkflowState.Name);
    }

    [Fact]
    public void ShouldReturnLocalAuthoritySearch_WhenLocalAuthorityNotProvided()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, name: "some name", planningDetails: _planningDetails);

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

    private void Test(SiteWorkflowState expected, SitePlanningDetails? planningDetails = null, SiteTenderingStatusDetails? tenderingStatusDetails = null)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            name: "some name",
            localAuthority: _localAuthority,
            planningDetails: planningDetails ?? _planningDetails,
            tenderingStatusDetails: tenderingStatusDetails ?? _tenderingStatusDetails);

        // when
        var result = workflow.CurrentState(SiteWorkflowState.Start);

        // then
        result.Should().Be(expected);
    }
}
