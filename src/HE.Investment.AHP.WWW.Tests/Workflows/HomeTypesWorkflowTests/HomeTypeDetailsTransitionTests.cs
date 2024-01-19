using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.HomeTypesWorkflowTests;

public class HomeTypeDetailsTransitionTests
{
    [Theory]
    [InlineData(HousingType.Undefined)]
    [InlineData(HousingType.General)]
    public async Task ShouldNavigateForward_WhenHousingTypeIsGeneral(HousingType housingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(housingType).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeTypeDetails);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.HomeInformation);
        states[1].Should().Be(HomeTypesWorkflowState.MoveOnAccommodation);
        states[2].Should().Be(HomeTypesWorkflowState.BuildingInformation);
    }

    [Theory]
    [InlineData(HousingType.Undefined)]
    [InlineData(HousingType.General)]
    public async Task ShouldNavigateBackward_WhenHousingTypeIsGeneral(HousingType housingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(housingType).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.BuildingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.MoveOnAccommodation);
        states[1].Should().Be(HomeTypesWorkflowState.HomeInformation);
        states[2].Should().Be(HomeTypesWorkflowState.HomeTypeDetails);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenHousingTypeIsHomesForOlderPeople()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(HousingType.HomesForOlderPeople).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeTypeDetails);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.HomesForOlderPeople);
        states[1].Should().Be(HomeTypesWorkflowState.HappiDesignPrinciples);
        states[2].Should().Be(HomeTypesWorkflowState.DesignPlans);
        states[3].Should().Be(HomeTypesWorkflowState.SupportedHousingInformation);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenHousingTypeIsHomesForOlderPeople()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(HousingType.HomesForOlderPeople).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.SupportedHousingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.DesignPlans);
        states[1].Should().Be(HomeTypesWorkflowState.HappiDesignPrinciples);
        states[2].Should().Be(HomeTypesWorkflowState.HomesForOlderPeople);
        states[3].Should().Be(HomeTypesWorkflowState.HomeTypeDetails);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenHousingTypeIsHomesForDisabledPeople()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(HousingType.HomesForDisabledAndVulnerablePeople).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeTypeDetails);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.HomesForDisabledPeople);
        states[1].Should().Be(HomeTypesWorkflowState.DisabledPeopleClientGroup);
        states[2].Should().Be(HomeTypesWorkflowState.HappiDesignPrinciples);
        states[3].Should().Be(HomeTypesWorkflowState.DesignPlans);
        states[4].Should().Be(HomeTypesWorkflowState.SupportedHousingInformation);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenHousingTypeIsHomesForDisabledPeople()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(HousingType.HomesForDisabledAndVulnerablePeople).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.SupportedHousingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.DesignPlans);
        states[1].Should().Be(HomeTypesWorkflowState.HappiDesignPrinciples);
        states[2].Should().Be(HomeTypesWorkflowState.DisabledPeopleClientGroup);
        states[3].Should().Be(HomeTypesWorkflowState.HomesForDisabledPeople);
        states[4].Should().Be(HomeTypesWorkflowState.HomeTypeDetails);
    }

    [Theory]
    [InlineData(HousingType.HomesForDisabledAndVulnerablePeople)]
    [InlineData(HousingType.HomesForOlderPeople)]
    public async Task ShouldNavigateViaParticularGroupState_WhenHousingTypeIs(HousingType housingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithHousingType(housingType).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures);
        states[1].Should().Be(HomeTypesWorkflowState.BuildingInformation);
        states[2].Should().Be(HomeTypesWorkflowState.PeopleGroupForSpecificDesignFeatures);
        states[3].Should().Be(HomeTypesWorkflowState.HomeInformation);
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType, false);
    }
}
