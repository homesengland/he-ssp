using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investments.Common.Contract;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SchemeWorkflowTests;

public class StateCanBeAccessedTests
{
    [Theory]
    [InlineData(SchemeWorkflowState.Start, true)]
    [InlineData(SchemeWorkflowState.Funding, true)]
    [InlineData(SchemeWorkflowState.PartnerDetails, false)]
    [InlineData(SchemeWorkflowState.Affordability, false)]
    [InlineData(SchemeWorkflowState.SalesRisk, false)]
    [InlineData(SchemeWorkflowState.HousingNeeds, true)]
    [InlineData(SchemeWorkflowState.StakeholderDiscussions, true)]
    [InlineData(SchemeWorkflowState.CheckAnswers, true)]
    public async Task ShouldReturnValue_WhenMethodCalledForDefaults(SchemeWorkflowState state, bool expectedResult)
    {
        // given
        var workflow = SchemeWorkflowFactory.BuildWorkflow(SchemeWorkflowState.Start);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(SchemeWorkflowState.Affordability, Tenure.SharedOwnership)]
    [InlineData(SchemeWorkflowState.SalesRisk, Tenure.SharedOwnership)]
    [InlineData(SchemeWorkflowState.Affordability, Tenure.OlderPersonsSharedOwnership)]
    [InlineData(SchemeWorkflowState.SalesRisk, Tenure.OlderPersonsSharedOwnership)]
    public async Task ShouldReturnValue_WhenTenureIsSharedOwnership(SchemeWorkflowState state, Tenure tenure)
    {
        // given
        var applicationDetails = new ApplicationDetails(
            new FrontDoorProjectId("project-1234"),
            new AhpApplicationId("test-1234"),
            "appName",
            tenure,
            ApplicationStatus.Draft,
            [AhpApplicationOperation.Modification]);
        var workflow = SchemeWorkflowFactory.BuildWorkflow(SchemeWorkflowState.Start, applicationDetails);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(true);
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledForPartnerDetailsAndOrganisationIsConsortiumMember()
    {
        // given
        var workflow = SchemeWorkflowFactory.BuildWorkflow(SchemeWorkflowState.Start, isConsortiumMember: true);

        // when
        var result = await workflow.StateCanBeAccessed(SchemeWorkflowState.PartnerDetails);

        // then
        result.Should().Be(true);
    }
}
