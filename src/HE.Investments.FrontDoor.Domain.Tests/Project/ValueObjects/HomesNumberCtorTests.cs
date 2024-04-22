using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects;

public class HomesNumberCtorTests
{
    private const string DisplayName = "number of homes your project will enable";

    private const int MinValue = 0;

    private const int MaxValue = 9999;

    [Fact]
    public void ShouldCreateHomesNumber_WhenProvidedValueIsWithinRange()
    {
        // given
        var homesNumber = 1;

        // when
        var result = () => new HomesNumber(homesNumber);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Value.Should().Be(homesNumber);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsNotProvided()
    {
        // given && when
        var result = () => new HomesNumber(null);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustProvideRequiredField(DisplayName));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsLessThanZero()
    {
        // given
        var homesNumber = -1;

        // when
        var result = () => new HomesNumber(homesNumber);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustProvideTheHigherNumber(DisplayName, MinValue));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsGreaterThanMaxValue()
    {
        // given
        var homesNumber = 1000000001;

        // when
        var result = () => new HomesNumber(homesNumber);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustProvideTheLowerNumber(DisplayName, MaxValue));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsNotANumber()
    {
        // given
        var homesNumber = "not a number";

        // when
        var result = () => new HomesNumber(homesNumber);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("The number of homes your project will enable must be a whole number, like 300");
    }
}
