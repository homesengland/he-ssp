using FluentAssertions;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.FrontDoor.Contract.Project.Enums;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects.SupportActivitiesTests;

public class SupportActivitiesCtorTests
{
    [Fact]
    public void ShouldCreateSupportActivities_WhenProvidedValueIsNotNull()
    {
        // given
        var supportActivityTypes = new[] { SupportActivityType.AcquiringLand };

        // when
        var result = () => new SupportActivities(supportActivityTypes);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Values.Should().BeEquivalentTo(supportActivityTypes);
    }

    [Fact]
    public void ShouldCreateSupportActivities_WhenDuplicatedTypesAreProvided()
    {
        // given
        var supportActivityTypes = new[] { SupportActivityType.AcquiringLand, SupportActivityType.AcquiringLand };

        // when
        var result = () => new SupportActivities(supportActivityTypes);

        // then
        result.Should().NotThrow<DomainValidationException>();
        result().Values.Should().BeEquivalentTo(new[] { SupportActivityType.AcquiringLand });
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenProvidedValueIsEmpty()
    {
        // given && when
        var result = () => new SupportActivities(new List<SupportActivityType>());

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("Select activities you require support for, or select ‘other'");
    }

    [Fact]
    public void ShouldCreateEmptySupportActivities_WhenNoValueIsProvided()
    {
        // given && when
        var result = SupportActivities.Empty();

        // then
        result.Values.Should().BeEmpty();
    }
}
