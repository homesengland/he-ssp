using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects;

public class IsSiteIdentifiedCtorTests
{
    [Fact]
    public void ShouldCreateIsSiteIdentified_WhenProvidedValueIsTrue()
    {
        // given
        var isSiteIdentified = true;

        // when
        var result = () => new IsSiteIdentified(isSiteIdentified);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Value.Should().Be(isSiteIdentified);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenIsSiteIdentifiedIsNotProvided()
    {
        // given && when
        var result = () => new IsSiteIdentified(null);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("Select yes if you have an identified site");
    }
}
