using FluentAssertions;
using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.SiteRuralClassificationTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenAllDataProvided()
    {
        // given
        var details = new SiteRuralClassification(true, false);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsWithinRuralSettlementNotProvided()
    {
        // given
        var details = new SiteRuralClassification(null, false);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenIsRuralExceptionSiteNotProvided()
    {
        // given
        var details = new SiteRuralClassification(true, null);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
