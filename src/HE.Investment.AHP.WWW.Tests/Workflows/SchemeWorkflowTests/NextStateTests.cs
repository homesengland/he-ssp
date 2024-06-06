using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SchemeWorkflowTests;

public class NextStateTests
{
    [Theory]
    [InlineData(SchemeWorkflowState.Start, SchemeWorkflowState.Funding)]
    [InlineData(SchemeWorkflowState.Affordability, SchemeWorkflowState.SalesRisk)]
    [InlineData(SchemeWorkflowState.SalesRisk, SchemeWorkflowState.HousingNeeds)]
    [InlineData(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.StakeholderDiscussions)]
    [InlineData(SchemeWorkflowState.StakeholderDiscussions, SchemeWorkflowState.CheckAnswers)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SchemeWorkflowState current, SchemeWorkflowState expectedNext)
    {
        // given
        var workflow = SchemeWorkflowFactory.BuildWorkflow(current);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SchemeWorkflowState.Funding, SchemeWorkflowState.Start)]
    [InlineData(SchemeWorkflowState.PartnerDetails, SchemeWorkflowState.Funding)]
    [InlineData(SchemeWorkflowState.SalesRisk, SchemeWorkflowState.Affordability)]
    [InlineData(SchemeWorkflowState.StakeholderDiscussions, SchemeWorkflowState.HousingNeeds)]
    [InlineData(SchemeWorkflowState.CheckAnswers, SchemeWorkflowState.StakeholderDiscussions)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(SchemeWorkflowState current, SchemeWorkflowState expectedNext)
    {
        // given
        var workflow = SchemeWorkflowFactory.BuildWorkflow(current);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SchemeWorkflowState.Funding, SchemeWorkflowState.Affordability, Tenure.SharedOwnership, false)]
    [InlineData(SchemeWorkflowState.Funding, SchemeWorkflowState.Affordability, Tenure.OlderPersonsSharedOwnership, false)]
    [InlineData(SchemeWorkflowState.Funding, SchemeWorkflowState.HousingNeeds, Tenure.AffordableRent, false)]
    [InlineData(SchemeWorkflowState.Funding, SchemeWorkflowState.PartnerDetails, Tenure.AffordableRent, true)]
    [InlineData(SchemeWorkflowState.PartnerDetails, SchemeWorkflowState.Affordability, Tenure.SharedOwnership, false)]
    [InlineData(SchemeWorkflowState.PartnerDetails, SchemeWorkflowState.Affordability, Tenure.OlderPersonsSharedOwnership, false)]
    [InlineData(SchemeWorkflowState.PartnerDetails, SchemeWorkflowState.HousingNeeds, Tenure.AffordableRent, false)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForSpecificTenureAndConsortiumMembership(
        SchemeWorkflowState current,
        SchemeWorkflowState expectedNext,
        Tenure tenure,
        bool isConsortiumMember)
    {
        // given
        var applicationDetails = new ApplicationDetails(
            new AhpApplicationId("test-1234"),
            "appName",
            tenure,
            ApplicationStatus.Draft,
            [AhpApplicationOperation.Modification]);
        var workflow = SchemeWorkflowFactory.BuildWorkflow(current, applicationDetails, isConsortiumMember: isConsortiumMember);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedNext);
    }

    [Theory]
    [InlineData(SchemeWorkflowState.Affordability, SchemeWorkflowState.Funding, Tenure.SharedOwnership, false)]
    [InlineData(SchemeWorkflowState.Affordability, SchemeWorkflowState.PartnerDetails, Tenure.SharedOwnership, true)]
    [InlineData(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.SalesRisk, Tenure.SharedOwnership, true)]
    [InlineData(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.SalesRisk, Tenure.OlderPersonsSharedOwnership, true)]
    [InlineData(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.PartnerDetails, Tenure.AffordableRent, true)]
    [InlineData(SchemeWorkflowState.HousingNeeds, SchemeWorkflowState.Funding, Tenure.AffordableRent, false)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedForSpecificTenureAndConsortiumMembership(
        SchemeWorkflowState current,
        SchemeWorkflowState expectedNext,
        Tenure tenure,
        bool isConsortiumMember)
    {
        // given
        var applicationDetails = new ApplicationDetails(
            new AhpApplicationId("test-1234"),
            "appName",
            tenure,
            ApplicationStatus.Draft,
            [AhpApplicationOperation.Modification]);
        var workflow = SchemeWorkflowFactory.BuildWorkflow(current, applicationDetails, isConsortiumMember: isConsortiumMember);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedNext);
    }
}
