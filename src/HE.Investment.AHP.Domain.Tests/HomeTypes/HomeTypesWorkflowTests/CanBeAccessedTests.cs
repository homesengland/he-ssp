using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.HomeTypesWorkflowTests;

public class CanBeAccessedTests
{
    [Fact]
#pragma warning disable S2699 // Tests should include assertions
    public void ShouldNotThrowException_ForEachPossibleWorkflowState()
#pragma warning restore S2699 // Tests should include assertions
    {
        // given
        var homeType = new HomeTypeTestDataBuilder().Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);
        var workflowStates = Enum.GetValues<HomeTypesWorkflowState>();

        // when
        foreach (var workflowState in workflowStates)
        {
            workflow.CanBeAccessed(workflowState);
        }

        // then - no Exception should be thrown
    }

    [Fact]
    public void ShouldReturnFalse_WhenBuildingInformationIsNotEligibleAndStateIsAfter()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.General)
            .WithBuildingType(BuildingType.Bedsit)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.CheckAnswers);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenBuildingInformationIsNotEligibleAndStateIsBefore()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.General)
            .WithBuildingType(BuildingType.Bedsit)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.HomeInformation);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrue_WhenBuildingInformationIsEligibleAndStateIsAfter()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithHousingType(HousingType.General)
            .WithBuildingType(BuildingType.House)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.CheckAnswers);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.RentToBuy)]
    public void ShouldReturnFalse_WhenProspectiveRentIsNotEligibleAndStateIsAfter(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.CheckAnswers);

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.RentToBuy)]
    public void ShouldReturnTrue_WhenProspectiveRentIsEligibleAndStateIsAfter(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.CheckAnswers);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.RentToBuy)]
    public void ShouldReturnTrue_WhenProspectiveRentIsNotEligibleAndStateIsBefore(Tenure tenure)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.FloorArea);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSharedOwnershipIsNotEligibleAndStateIsAfter()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SharedOwnership)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.CheckAnswers);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenSharedOwnershipIsEligibleAndStateIsAfter()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SharedOwnership)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.CheckAnswers);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrue_WhenSharedOwnershipIsNotEligibleAndStateIsBefore()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(Tenure.SharedOwnership)
            .WithProspectiveRentIneligible()
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.Index);

        // when
        var result = workflow.CanBeAccessed(HomeTypesWorkflowState.FloorArea);

        // then
        result.Should().BeTrue();
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType, false);
    }
}
