using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Planning.LandRegistryDetailsTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenTitleNumberNotRegistered()
    {
        // given
        var details = new LandRegistryDetails(false, null, null);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrue_WhenTitleNumberRegistered()
    {
        // given
        var details = new LandRegistryDetails(true, new LandRegistryTitleNumber("12"), true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenTitleNumberMissing()
    {
        // given
        var details = new LandRegistryDetails(true, null, true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsCoveredMissing()
    {
        // given
        var details = new LandRegistryDetails(true, new LandRegistryTitleNumber("12"), null);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
