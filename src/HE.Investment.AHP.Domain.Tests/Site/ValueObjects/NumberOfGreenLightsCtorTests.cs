using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects;

public class NumberOfGreenLightsCtorTests
{
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
            .WithMessage("The value you enter for the Building for Life green traffic lights must be a whole number");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsLessThanMinValue()
    {
        // given && when
        var result = () => new NumberOfGreenLights("-1");

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithMessage("The value you enter for the Building for Life green traffic lights must be 0 or more");
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsGreaterThanMaxValue()
    {
        // given && when
        var result = () => new NumberOfGreenLights("13");

        // then
        result.Should()
            .Throw<DomainValidationException>()
            .WithMessage("The value you enter for the Building for Life green traffic lights must be 12 or fewer");
    }
}
