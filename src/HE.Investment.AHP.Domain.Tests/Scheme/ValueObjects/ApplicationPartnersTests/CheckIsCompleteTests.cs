using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Scheme.ValueObjects.ApplicationPartnersTests;

public class CheckIsCompleteTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(null)]
    public void ShouldThrowException_WhenApplicationPartnersAreNotConfirmed(bool? isConfirmed)
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithPartnersConfirmation(isConfirmed)
            .Build();

        // when
        var check = () => testCandidate.CheckIsComplete();

        // then
        check.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldNotThrowException_WhenApplicationPartnersAreConfirmed()
    {
        // given
        var testCandidate = ApplicationPartnersBuilder.New()
            .WithPartnersConfirmation(true)
            .Build();

        // when
        var check = () => testCandidate.CheckIsComplete();

        // then
        check.Should().NotThrow();
    }
}
