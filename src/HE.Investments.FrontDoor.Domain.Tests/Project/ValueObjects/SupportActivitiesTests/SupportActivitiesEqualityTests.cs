using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ValueObjects.SupportActivitiesTests;

public class SupportActivitiesEqualityTests
{
    [Fact]
    public void ShouldTwoSupportActivitiesBeEqual_WhenThereIsOneElement()
    {
        // given
        var first = new SupportActivities([SupportActivityType.AcquiringLand]);
        var second = new SupportActivities([SupportActivityType.AcquiringLand]);

        // when
        var result = first == second;

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldTwoSupportActivitiesBeEqual_WhenThereAreMoreThanOneElement()
    {
        // given
        var first = new SupportActivities([SupportActivityType.AcquiringLand, SupportActivityType.DevelopingHomes]);
        var second = new SupportActivities([SupportActivityType.DevelopingHomes, SupportActivityType.AcquiringLand]);

        // when
        var result = first == second;

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldTwoSupportActivitiesBeNotEqual_WhenThereAreMoreThanOneElement()
    {
        // given
        var first = new SupportActivities([SupportActivityType.SellingLandToHomesEngland, SupportActivityType.DevelopingHomes]);
        var second = new SupportActivities([SupportActivityType.DevelopingHomes, SupportActivityType.AcquiringLand]);

        // when
        var result = first == second;

        // then
        result.Should().BeFalse();
    }
}
