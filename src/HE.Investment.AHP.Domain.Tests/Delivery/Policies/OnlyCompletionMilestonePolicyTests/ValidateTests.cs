using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Policies.OnlyCompletionMilestonePolicyTests;

public class ValidateTests
{
    [Theory]
    [InlineData(false, BuildActivityType.OffTheShelf)]
    [InlineData(true, BuildActivityType.WorksOnly)]
    [InlineData(true, BuildActivityType.OffTheShelf)]
    public void ShouldReturnTrue_WhenIsUnregisteredBodyOrBuildActivityIsOffTheShelf(bool isUnregisteredBody, BuildActivityType buildActivity)
    {
        // given
        var testCandidate = new OnlyCompletionMilestonePolicy();

        // when
        var result = testCandidate.Validate(isUnregisteredBody, new BuildActivity(Tenure.Undefined, type: buildActivity));

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsRegisteredBodyAndBuildActivityIsWorksOnly()
    {
        // given
        var testCandidate = new OnlyCompletionMilestonePolicy();

        // when
        var result = testCandidate.Validate(false, new BuildActivity(Tenure.Undefined, type: BuildActivityType.WorksOnly));

        // then
        result.Should().BeFalse();
    }
}
