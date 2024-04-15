using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects;

public class NumberOfGreenLightsCtorTests
{
    private readonly string _displayName = "value you enter for the Building for Life green traffic lights";

    [Fact]
    public void ShouldCreateNumberOfGreenLights_WhenProvidedValueIsWithinRange()
    {
        // given
        var value = "5";

        // when
        var result = new NumberOfGreenLights(value);

        // then
        result.Value.Should().Be(5);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsNotWholeNumber()
    {
        // given && when
        var result = () => new NumberOfGreenLights("5.5");

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithMessage(ValidationErrorMessage.MustBeWholeNumber(_displayName));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsLessThanMinValue()
    {
        // given && when
        var result = () => new NumberOfGreenLights("-1");

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithMessage(ValidationErrorMessage.MustBeNumberBetween(_displayName, 0, 12));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsGreaterThanMaxValue()
    {
        // given && when
        var result = () => new NumberOfGreenLights("13");

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithMessage(ValidationErrorMessage.MustBeNumberBetween(_displayName, 0, 12));
    }
}
