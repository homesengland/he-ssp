using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Loans.Common.Routing;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.HomeTypesWorkflowTests;

public class TenureTransitionTests
{
    [Fact]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreNotMetAndTenureIsAffordableRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.AffordableRent)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[1].Should().Be(HomeTypesWorkflowState.AffordableRent);
        states[2].Should().Be(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreNotMetAndTenureIsAffordableRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.AffordableRent)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.AffordableRent);
        states[1].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[2].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreMetAndTenureIsAffordableRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.AffordableRent)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.AffordableRent);
        states[1].Should().Be(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreMetAndTenureIsAffordableRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.AffordableRent)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.AffordableRent);
        states[1].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenAffordableRentIsNotEligible()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.AffordableRent)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.AffordableRent);

        // when
        var state = await workflow.NextState(Trigger.Continue);
        var nextState = () => workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.AffordableRentIneligible);
        await nextState.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.SocialRent)]
    public async Task ShouldNavigateForward_WhenExemptFromRightIsYesAndTenureIs(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithExemptFromTheRightToSharedOwnership(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ExemptionJustification);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.SocialRent)]
    public async Task ShouldNavigateBackward_WhenExemptFromRightIsYesAndTenureIs(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithExemptFromTheRightToSharedOwnership(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ExemptionJustification);
        states[1].Should().Be(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreNotMetAndTenureIsSocialRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SocialRent)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[1].Should().Be(HomeTypesWorkflowState.SocialRent);
        states[2].Should().Be(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreNotMetAndTenureIsSocialRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SocialRent)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.SocialRent);
        states[1].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[2].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreMetAndTenureIsSocialRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SocialRent)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.SocialRent);
        states[1].Should().Be(HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreMetAndTenureIsSocialRent()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SocialRent)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.SocialRent);
        states[1].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Theory]
    [InlineData(Tenure.SharedOwnership, HomeTypesWorkflowState.SharedOwnership)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities, HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreNotMetAndTenureIs(Tenure tenure, HomeTypesWorkflowState nextWorkflowState)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[1].Should().Be(nextWorkflowState);
        states[2].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreNotMetAndTenureIsSharedOwnership()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SharedOwnership)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.SharedOwnership);
        states[1].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[2].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Theory]
    [InlineData(Tenure.SharedOwnership, HomeTypesWorkflowState.SharedOwnership)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities, HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreMetAndTenureIs(Tenure tenure, HomeTypesWorkflowState nextWorkflowState)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(nextWorkflowState);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Theory]
    [InlineData(Tenure.SharedOwnership, HomeTypesWorkflowState.SharedOwnership)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities, HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreMetAndTenureIs(Tenure tenure, HomeTypesWorkflowState previousWorkflowState)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(previousWorkflowState);
        states[1].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Theory]
    [InlineData(Tenure.SharedOwnership, HomeTypesWorkflowState.SharedOwnership)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities, HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    public async Task ShouldNavigateForward_WhenProspectiveRentIsNotEligibleAndTenureIs(Tenure tenure, HomeTypesWorkflowState currentWorkflowState)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, currentWorkflowState);

        // when
        var state = await workflow.NextState(Trigger.Continue);
        var nextState = () => workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.ProspectiveRentIneligible);
        await nextState.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreNotMetAndTenureIsRentToBuy()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.RentToBuy)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[1].Should().Be(HomeTypesWorkflowState.RentToBuy);
        states[2].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreNotMetAndTenureIsRentToBuy()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.RentToBuy)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.RentToBuy);
        states[1].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[2].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenSpaceStandardsAreMetAndTenureIsRentToBuy()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.RentToBuy)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.RentToBuy);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenSpaceStandardsAreMetAndTenureIsRentToBuy()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.RentToBuy)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.RentToBuy);
        states[1].Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenRentToBuyIsNotEligible()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.RentToBuy)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.RentToBuy);

        // when
        var state = await workflow.NextState(Trigger.Continue);
        var nextState = () => workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.RentToBuyIneligible);
        await nextState.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public async Task ShouldNavigateForwardToCheckAnswers_WhenSpaceStandardsAreNotMetAndTenureIs(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Theory]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public async Task ShouldNavigateBackwardFromCheckAnswers_WhenSpaceStandardsAreNotMetAndTenureIs(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var state = await workflow.NextState(Trigger.Back);

        // then
        state.Should().Be(HomeTypesWorkflowState.FloorAreaStandards);
    }

    [Theory]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public async Task ShouldNavigateForwardToCheckAnswers_WhenSpaceStandardsAreMetAndTenureIs(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.FloorArea);

        // when
        var state = await workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Theory]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public async Task ShouldNavigateBackwardFromCheckAnswers_WhenSpaceStandardsAreMetAndTenureIs(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSpaceStandardsMet(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var state = await workflow.NextState(Trigger.Back);

        // then
        state.Should().Be(HomeTypesWorkflowState.FloorArea);
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType, false);
    }
}
