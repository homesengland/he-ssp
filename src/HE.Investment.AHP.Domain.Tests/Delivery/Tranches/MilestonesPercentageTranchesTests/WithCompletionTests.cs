using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Tranches.MilestonesPercentageTranchesTests;

public class WithCompletionTests
{
    [Theory]
    [InlineData(0.95)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    public void ShouldReturnNewInstanceWithCompletion_WhenCompletionIsLessThanMaxCompletionTranche(decimal completion)
    {
        // given
        var milestonesPercentageTranches = new MilestonesPercentageTranches(null, null, null);
        var completionPercentage = new WholePercentage(completion);

        // when
        var result = milestonesPercentageTranches.WithCompletion(completionPercentage);

        // then
        result.Completion.Should().Be(completionPercentage);
    }

    [Theory]
    [InlineData(0.04)]
    [InlineData(0.0)]
    [InlineData(1.01)]
    [InlineData(1.1)]
    [InlineData(2.0)]
    public void ShouldThrowValidationError_WhenCompletionIsLessThanMinimalCompletionTrancheOrMoreThanMaxCompletionTranche(decimal completion)
    {
        // given
        var milestonesPercentageTranches = new MilestonesPercentageTranches(null, null, null);
        var completionPercentage = new WholePercentage(completion);

        // when
        var result = () => milestonesPercentageTranches.WithCompletion(completionPercentage);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("Completion tranche must be between 5% and 100% of the grant apportioned");
    }
}
