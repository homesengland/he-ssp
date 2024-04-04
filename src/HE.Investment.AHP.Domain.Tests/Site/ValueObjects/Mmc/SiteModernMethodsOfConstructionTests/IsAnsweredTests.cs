using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using SiteModernMethodsOfConstruction = HE.Investment.AHP.Domain.Site.ValueObjects.Mmc.SiteModernMethodsOfConstruction;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Mmc.SiteModernMethodsOfConstructionTests;

public class IsAnsweredTests
{
    private readonly ModernMethodsOfConstructionPlans _modernMethodsOfConstructionPlans = new("rzucic palenie");
    private readonly ModernMethodsOfConstructionExpectedImpact _modernMethodsOfConstructionExpectedImpact = new("expected impact");
    private readonly ModernMethodsOfConstructionBarriers _modernMethodsOfConstructionBarriers = new("brak kasy");
    private readonly ModernMethodsOfConstructionImpact _modernMethodsOfConstructionImpact = new("świetlana przyszłosć");
    private readonly ModernMethodsOfConstruction _modernMethodsOfConstruction = ModernMethodsOfConstructionBuilder.New().BuildValid2DAnd3DCategories();

    [Fact]
    public void ShouldReturnTrue_WhenAllDataProvidedForNoSiteUsingModernMethodsOfConstruction()
    {
        // given
        var details = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.No,
            null,
            new ModernMethodsOfConstructionFutureAdoption(_modernMethodsOfConstructionPlans, _modernMethodsOfConstructionExpectedImpact));

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenModernMethodsOfConstructionFutureAdoptionNotProvided()
    {
        // given
        var details = new SiteModernMethodsOfConstruction(SiteUsingModernMethodsOfConstruction.No);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenAllDataProvidedForSiteUsingModernMethodsOfConstruction()
    {
        // given
        var details = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new ModernMethodsOfConstructionInformation(_modernMethodsOfConstructionBarriers, _modernMethodsOfConstructionImpact),
            null,
            _modernMethodsOfConstruction);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(SiteUsingModernMethodsOfConstruction.Yes)]
    [InlineData(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)]
    public void ShouldReturnFalse_WhenSiteUsingModernMethodsOfConstructionInformationNotProvided(SiteUsingModernMethodsOfConstruction usingMmc)
    {
        // given
        var details = new SiteModernMethodsOfConstruction(
            usingMmc,
            null,
            null,
            _modernMethodsOfConstruction);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSiteUsingModernMethodsOfConstructionModernMethodsOfConstructionNotProvided()
    {
        // given
        var details = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new ModernMethodsOfConstructionInformation(_modernMethodsOfConstructionBarriers, _modernMethodsOfConstructionImpact));

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenAllDataProvidedForSiteUsingModernMethodsOfConstructionOnlyForSomeHomes()
    {
        // given
        var details = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes,
            new ModernMethodsOfConstructionInformation(_modernMethodsOfConstructionBarriers, _modernMethodsOfConstructionImpact));

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSiteUsingModernMethodsOfConstructionNotProvided()
    {
        // given
        var details = new SiteModernMethodsOfConstruction();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
