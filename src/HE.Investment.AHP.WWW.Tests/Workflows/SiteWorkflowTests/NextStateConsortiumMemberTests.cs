using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class NextStateConsortiumMemberTests
{
    [Theory]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, SiteWorkflowState.DevelopingPartner)]
    [InlineData(SiteWorkflowState.NumberOfGreenLights, SiteWorkflowState.DevelopingPartner)]
    [InlineData(SiteWorkflowState.DevelopingPartner, SiteWorkflowState.DevelopingPartnerConfirm)]
    [InlineData(SiteWorkflowState.DevelopingPartnerConfirm, SiteWorkflowState.OwnerOfTheLand)]
    [InlineData(SiteWorkflowState.OwnerOfTheLand, SiteWorkflowState.OwnerOfTheLandConfirm)]
    [InlineData(SiteWorkflowState.OwnerOfTheLandConfirm, SiteWorkflowState.OwnerOfTheHomes)]
    [InlineData(SiteWorkflowState.OwnerOfTheHomes, SiteWorkflowState.OwnerOfTheHomesConfirm)]
    [InlineData(SiteWorkflowState.OwnerOfTheHomesConfirm, SiteWorkflowState.LandAcquisitionStatus)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SiteWorkflowState currentState, SiteWorkflowState expectedState)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(currentState, isConsortiumMember: true);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedState);
    }

    [Theory]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, SiteWorkflowState.OwnerOfTheHomes)]
    [InlineData(SiteWorkflowState.OwnerOfTheHomesConfirm, SiteWorkflowState.OwnerOfTheHomes)]
    [InlineData(SiteWorkflowState.OwnerOfTheHomes, SiteWorkflowState.OwnerOfTheLand)]
    [InlineData(SiteWorkflowState.OwnerOfTheLandConfirm, SiteWorkflowState.OwnerOfTheLand)]
    [InlineData(SiteWorkflowState.OwnerOfTheLand, SiteWorkflowState.DevelopingPartner)]
    [InlineData(SiteWorkflowState.DevelopingPartnerConfirm, SiteWorkflowState.DevelopingPartner)]
    [InlineData(SiteWorkflowState.DevelopingPartner, SiteWorkflowState.BuildingForHealthyLife)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedAndNoBuildingForHealthyLife(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        await TestBack(current, expectedNext, BuildingForHealthyLifeType.No);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenBackTriggerExecutedAndBuildingForHealthyLife()
    {
        await TestBack(SiteWorkflowState.DevelopingPartner, SiteWorkflowState.NumberOfGreenLights, BuildingForHealthyLifeType.Yes);
    }

    private static async Task TestBack(
        SiteWorkflowState currentState,
        SiteWorkflowState expectedState,
        BuildingForHealthyLifeType buildingForHealthyLife)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            currentState,
            isConsortiumMember: true,
            buildingForHealthyLife: buildingForHealthyLife);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedState);
    }
}
