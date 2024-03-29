using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.HomeTypesWorkflowTests;

public class SupportedHousingInformationTransitionTests
{
    [Theory]
    [InlineData(RevenueFundingType.RevenueFundingNotNeeded)]
    [InlineData(RevenueFundingType.RevenueFundingNeededButNotIdentified)]
    public async Task ShouldNavigateForward_WhenShortStayIsNoAndRevenueFundingIs(RevenueFundingType revenueFundingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForDisabledAndVulnerablePeople)
            .WithShortStayAccommodation(YesNoType.No)
            .WithRevenueFundingType(revenueFundingType)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.SupportedHousingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[1].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[2].Should().Be(HomeTypesWorkflowState.HomeInformation);
    }

    [Theory]
    [InlineData(RevenueFundingType.RevenueFundingNotNeeded)]
    [InlineData(RevenueFundingType.RevenueFundingNeededButNotIdentified)]
    public async Task ShouldNavigateBackward_WhenShortStayIsNoAndRevenueFundingIs(RevenueFundingType revenueFundingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForOlderPeople)
            .WithShortStayAccommodation(YesNoType.No)
            .WithRevenueFundingType(revenueFundingType)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[1].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[2].Should().Be(HomeTypesWorkflowState.SupportedHousingInformation);
    }

    [Theory]
    [InlineData(RevenueFundingType.RevenueFundingNotNeeded)]
    [InlineData(RevenueFundingType.RevenueFundingNeededButNotIdentified)]
    public async Task ShouldNavigateForward_WhenShortStayIsYesAndRevenueFundingIs(RevenueFundingType revenueFundingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForOlderPeople)
            .WithShortStayAccommodation(YesNoType.Yes)
            .WithRevenueFundingType(revenueFundingType)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.SupportedHousingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.MoveOnArrangements);
        states[1].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[2].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[3].Should().Be(HomeTypesWorkflowState.HomeInformation);
    }

    [Theory]
    [InlineData(RevenueFundingType.RevenueFundingNotNeeded)]
    [InlineData(RevenueFundingType.RevenueFundingNeededButNotIdentified)]
    public async Task ShouldNavigateBackward_WhenShortStayIsYesAndRevenueFundingIs(RevenueFundingType revenueFundingType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForDisabledAndVulnerablePeople)
            .WithShortStayAccommodation(YesNoType.Yes)
            .WithRevenueFundingType(revenueFundingType)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[1].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[2].Should().Be(HomeTypesWorkflowState.MoveOnArrangements);
        states[3].Should().Be(HomeTypesWorkflowState.SupportedHousingInformation);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenShortStayIsYesAndRevenueFundingIsIdentified()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForOlderPeople)
            .WithShortStayAccommodation(YesNoType.Yes)
            .WithRevenueFundingType(RevenueFundingType.RevenueFundingNeededAndIdentified)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.SupportedHousingInformation);

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
        states[0].Should().Be(HomeTypesWorkflowState.RevenueFunding);
        states[1].Should().Be(HomeTypesWorkflowState.MoveOnArrangements);
        states[2].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[3].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[4].Should().Be(HomeTypesWorkflowState.HomeInformation);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenShortStayIsYesAndRevenueFundingIsIdentified()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForDisabledAndVulnerablePeople)
            .WithShortStayAccommodation(YesNoType.Yes)
            .WithRevenueFundingType(RevenueFundingType.RevenueFundingNeededAndIdentified)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeInformation);

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
        states[0].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[1].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[2].Should().Be(HomeTypesWorkflowState.MoveOnArrangements);
        states[3].Should().Be(HomeTypesWorkflowState.RevenueFunding);
        states[4].Should().Be(HomeTypesWorkflowState.SupportedHousingInformation);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenShortStayIsNoAndRevenueFundingIsIdentified()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForOlderPeople)
            .WithShortStayAccommodation(YesNoType.No)
            .WithRevenueFundingType(RevenueFundingType.RevenueFundingNeededAndIdentified)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.SupportedHousingInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.RevenueFunding);
        states[1].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[2].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[3].Should().Be(HomeTypesWorkflowState.HomeInformation);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenShortStayIsNoAndRevenueFundingIsIdentified()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.HomesForDisabledAndVulnerablePeople)
            .WithShortStayAccommodation(YesNoType.No)
            .WithRevenueFundingType(RevenueFundingType.RevenueFundingNeededAndIdentified)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.HomeInformation);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.TypologyLocationAndDesign);
        states[1].Should().Be(HomeTypesWorkflowState.ExitPlan);
        states[2].Should().Be(HomeTypesWorkflowState.RevenueFunding);
        states[3].Should().Be(HomeTypesWorkflowState.SupportedHousingInformation);
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType, false);
    }
}
