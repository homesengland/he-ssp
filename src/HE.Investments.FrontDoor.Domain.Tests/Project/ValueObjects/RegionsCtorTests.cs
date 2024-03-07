using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects;

public class RegionsCtorTests
{
    [Fact]
    public void ShouldCreateRegions_WhenProvidedValueIsNotNull()
    {
        // given
        var regions = new[] { RegionType.London };

        // when
        var result = () => new Regions(regions);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Values.Should().BeEquivalentTo(regions);
    }

    [Fact]
    public void ShouldCreateRegions_WhenDuplicatedTypesAreProvided()
    {
        // given
        var regions = new[] { RegionType.WestMidlands, RegionType.WestMidlands };

        // when
        var result = () => new Regions(regions);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Values.Should().BeEquivalentTo(new[] { RegionType.WestMidlands });
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsEmpty()
    {
        // given && when
        var result = () => new Regions(new List<RegionType>());

        // then
        result.Should().Throw<DomainValidationException>().WithMessage(ValidationErrorMessage.MustBeSelected("region your project will be located in"));
    }

    [Fact]
    public void ShouldCreateEmptySupportActivities_WhenNoValueIsProvided()
    {
        // given && when
        var result = Regions.Empty();

        // then
        result.Values.Should().BeEmpty();
    }
}
