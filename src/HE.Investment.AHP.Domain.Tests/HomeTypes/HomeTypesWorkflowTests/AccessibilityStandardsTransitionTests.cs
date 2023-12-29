using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.HomeTypesWorkflowTests;

public class AccessibilityStandardsTransitionTests
{
    [Fact]
    public async Task ShouldNavigateForward_WhenAccessibilityStandardsIsYes()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithAccessibilityStandards(YesNoType.Yes).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.AccessibilityStandards);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.AccessibilityCategory);
        states[1].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenAccessibilityStandardsIsYes()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithAccessibilityStandards(YesNoType.Yes).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.AccessibilityCategory);
        states[1].Should().Be(HomeTypesWorkflowState.AccessibilityStandards);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenAccessibilityStandardsIsNo()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithAccessibilityStandards(YesNoType.No).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.AccessibilityStandards);

        // when
        var state = await workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenAccessibilityStandardsIsNo()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().WithAccessibilityStandards(YesNoType.No).Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var state = await workflow.NextState(Trigger.Back);

        // then
        state.Should().Be(HomeTypesWorkflowState.AccessibilityStandards);
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType, false);
    }
}
