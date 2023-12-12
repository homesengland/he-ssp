using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Loans.Common.Routing;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.HomeTypesWorkflowTests;

public class BuildingInformationTransitionTests
{
    [Theory]
    [InlineData(HousingType.Undefined)]
    [InlineData(HousingType.General)]
    public async Task ShouldNavigateToDeadEnd_WhenBuildingTypeIsBedsitAndHousingTypeIs(HousingType housingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(housingType)
            .WithBuildingType(BuildingType.Bedsit)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.BuildingInformation);

        // when
        var state = await workflow.NextState(Trigger.Continue);
        var nextState = () => workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.BuildingInformationIneligible);
        await nextState.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(HousingType.Undefined, BuildingType.House)]
    [InlineData(HousingType.General, BuildingType.Undefined)]
    [InlineData(HousingType.HomesForDisabledAndVulnerablePeople, BuildingType.Bedsit)]
    [InlineData(HousingType.HomesForDisabledAndVulnerablePeople, BuildingType.Bungalow)]
    [InlineData(HousingType.HomesForOlderPeople, BuildingType.Undefined)]
    [InlineData(HousingType.HomesForOlderPeople, BuildingType.Bedsit)]
    public async Task ShouldNavigateForward_WhenBuildingTypeAndHousingTypeIs(HousingType housingType, BuildingType buildingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(housingType)
            .WithBuildingType(buildingType)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.BuildingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.CustomBuildProperty);
        states[1].Should().Be(HomeTypesWorkflowState.TypeOfFacilities);
        states[2].Should().Be(HomeTypesWorkflowState.AccessibilityStandards);
    }

    [Fact]
    public async Task ShouldNavigateBackward()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.General)
            .WithBuildingType(BuildingType.Bungalow)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.AccessibilityStandards);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.TypeOfFacilities);
        states[1].Should().Be(HomeTypesWorkflowState.CustomBuildProperty);
        states[2].Should().Be(HomeTypesWorkflowState.BuildingInformation);
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType);
    }
}
