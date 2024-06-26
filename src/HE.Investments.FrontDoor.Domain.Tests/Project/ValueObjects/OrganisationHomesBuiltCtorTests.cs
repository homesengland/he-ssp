using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects;

public class OrganisationHomesBuiltCtorTests
{
    private const string DisplayName = "amount of homes your organisation has built in the last year";

    private const int MinValue = 0;

    private const int MaxValue = 9999;

    [Fact]
    public void ShouldCreateOrganisationHomesBuilt_WhenProvidedValueIsWithinRange()
    {
        // given
        var homesBuilt = 1;

        // when
        var result = () => new OrganisationHomesBuilt(homesBuilt);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Value.Should().Be(homesBuilt);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsNotProvided()
    {
        // given && when
        var result = () => new OrganisationHomesBuilt(null);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustProvideRequiredField(DisplayName));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsLessThanZero()
    {
        // given
        var homesBuilt = -1;

        // when
        var result = () => new OrganisationHomesBuilt(homesBuilt);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustProvideTheHigherNumber(DisplayName, MinValue));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsGreaterThanMaxValue()
    {
        // given
        var homesBuilt = 1000000001;

        // when
        var result = () => new OrganisationHomesBuilt(homesBuilt);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustProvideTheLowerNumber(DisplayName, MaxValue));
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsNotANumber()
    {
        // given
        var homesBuilt = "not a number";

        // when
        var result = () => new OrganisationHomesBuilt(homesBuilt);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("The amount of homes your organisation has built in the last year must be a whole number, like 300");
    }
}
