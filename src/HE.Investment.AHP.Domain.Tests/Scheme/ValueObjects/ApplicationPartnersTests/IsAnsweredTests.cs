extern alias Org;

using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenApplicationPartnersAreConfirmed()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithConfirmation(true)
            .Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(null)]
    public void ShouldReturnFalse_WhenApplicationPartnersAreNotConfirmed(bool? isConfirmed)
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithConfirmation(isConfirmed)
            .Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
