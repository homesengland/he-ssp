using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenAllQuestionsAreAnswered()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSection106IsNotAnswered()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithSection106(Section106Builder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenLocalAuthorityIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithLocalAuthority(null).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenPlanningDetailsAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithPlanningDetails(new EmptyPlanningDetails()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenNationalDesignGuidePrioritiesAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithNationalDesignGuidePriorities(NationalDesignGuidePrioritiesBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenBuildingForHealthyLifeIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithBuildingForHealthyLife(BuildingForHealthyLifeType.Undefined).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenNumberOfGreenLightsIsNotProvidedAndIsBuildingForHealthyLife()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithBuildingForHealthyLife(BuildingForHealthyLifeType.Yes).WithNumberOfGreenLights(null).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenNumberOfGreenLightsIsNotProvidedAndIsNotBuildingForHealthyLife()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithBuildingForHealthyLife(BuildingForHealthyLifeType.No).WithNumberOfGreenLights(null).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenLandAcquisitionStatusIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithLandAcquisitionStatus(LandAcquisitionStatusBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenTenderingStatusDetailsAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithTenderingStatusDetails(TenderingStatusDetailsBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenStrategicDetailsDetailsAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithStrategicDetails(StrategicSiteDetailsBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSiteTypeDetailsAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithSiteTypeDetails(SiteTypeDetailsBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSiteUseDetailsAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithSiteUseDetails(SiteUseDetailsBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenRuralClassificationIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithRuralClassification(SiteRuralClassificationBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenEnvironmentalImpactIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithEnvironmentalImpact(null).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenModernMethodsOfConstructionAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithModernMethodsOfConstruction(SiteModernMethodsOfConstructionBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenSiteProcurementsAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithProcurements(SiteProcurementsBuilder.New().Build()).Build();

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    private static SiteEntityBuilder CreateAnsweredSiteBuilder()
    {
        return SiteEntityBuilder.New()
            .WithSection106(Section106Builder.New().WithGeneralAgreement(false).Build())
            .WithLocalAuthority(LocalAuthorityBuilder.New().Build())
            .WithPlanningDetails(new NoPlanningRequiredPlanningDetails(new LandRegistryDetails(true, new LandRegistryTitleNumber("123"), true)))
            .WithNationalDesignGuidePriorities(NationalDesignGuidePrioritiesBuilder.New().WithPriorities(NationalDesignGuidePriority.Identity).Build())
            .WithBuildingForHealthyLife(BuildingForHealthyLifeType.No)
            .WithNumberOfGreenLights(null)
            .WithLandAcquisitionStatus(LandAcquisitionStatusBuilder.New().WithStatus(SiteLandAcquisitionStatus.FullOwnership).Build())
            .WithTenderingStatusDetails(TenderingStatusDetailsBuilder.New().WithStatus(SiteTenderingStatus.NotApplicable).Build())
            .WithStrategicDetails(StrategicSiteDetailsBuilder.New().WithIsStrategicSite(false).Build())
            .WithSiteTypeDetails(SiteTypeDetailsBuilder.New().WithSiteType(SiteType.NotApplicable).WithIsOnGreenBelt(true).WithIsRegenerationSite(true).Build())
            .WithSiteUseDetails(SiteUseDetailsBuilder.New().WithIsPartOfStreetFrontInfill(true).WithIsForTravellerPitchSite(false).Build())
            .WithRuralClassification(SiteRuralClassificationBuilder.New().WithIsRuralExceptionSite(true).WithIsWithinRuralSettlement(true).Build())
            .WithEnvironmentalImpact(new EnvironmentalImpact("impact"))
            .WithModernMethodsOfConstruction(SiteModernMethodsOfConstructionBuilder.New().WithIsUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes).Build())
            .WithProcurements(SiteProcurementsBuilder.New().WithProcurements(SiteProcurement.PartneringSupplyChain).Build());
    }
}
