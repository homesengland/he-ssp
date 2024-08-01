using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Strategies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Strategies.MilestoneAvailabilityStrategyTests;

public class OnlyCompletionMilestoneTests
{
    [Theory]
    [InlineData(false, BuildActivityType.OffTheShelf)]
    [InlineData(true, BuildActivityType.WorksOnly)]
    [InlineData(true, BuildActivityType.OffTheShelf)]
    public void ShouldReturnTrue_WhenIsUnregisteredBodyOrBuildActivityIsOffTheShelf(bool isUnregisteredBody, BuildActivityType buildActivity)
    {
        // given
        var testCandidate = new MilestoneAvailabilityStrategy();

        // when
        var result = testCandidate.OnlyCompletionMilestone(isUnregisteredBody, new BuildActivity(Tenure.Undefined, type: buildActivity));

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsRegisteredBodyAndBuildActivityIsWorksOnly()
    {
        // given
        var testCandidate = new MilestoneAvailabilityStrategy();

        // when
        var result = testCandidate.OnlyCompletionMilestone(false, new BuildActivity(Tenure.Undefined, type: BuildActivityType.WorksOnly));

        // then
        result.Should().BeFalse();
    }
}
