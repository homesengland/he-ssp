using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects;

public class DateValueObjectTests
{
    [Fact]
    public void ShouldThrowDomainValidationException_WhenValueIsInvalid()
    {
        // given && when
        var action = () => new DateValueObject("32", string.Empty, string.Empty, "test", "test");

        // then
        action.Should().ThrowExactly<DomainValidationException>();
    }

    [Fact]
    public void ShouldReturnDate_WhenValueIsValid()
    {
        // given && when
        var date = new DateValueObject("25", "2", "2033", "test", "test");

        // then
        date.Value.Day.Should().Be(25);
        date.Value.Month.Should().Be(2);
        date.Value.Year.Should().Be(2033);
    }
}
