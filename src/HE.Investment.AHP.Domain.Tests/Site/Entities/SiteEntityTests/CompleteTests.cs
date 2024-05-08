using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning.PlanningDetailsTypes;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class CompleteTests
{
    [Fact]
    public void ShouldThrowException_WhenAnswerIsUndefined()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().Build();

        // when
        var complete = () => testCandidate.Complete(IsSectionCompleted.Undefied);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be(nameof(IsSectionCompleted));
    }

    [Fact]
    public void ShouldThrowException_WhenAnswerIsYesAndNotAllQuestionsAreAnswered()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithLocalAuthority(null).Build();

        // when
        var complete = () => testCandidate.Complete(IsSectionCompleted.Yes);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be(nameof(IsSectionCompleted));
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenAnswerIsYesAndAllQuestionsAreAnswered()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().Build();

        // when
        testCandidate.Complete(IsSectionCompleted.Yes);

        // then
        testCandidate.Status.Should().Be(SiteStatus.Completed);
    }

    [Fact]
    public void ShouldChangeStatusToInProgress_WhenAnswerIsNo()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithStatus(SiteStatus.Completed).Build();

        // when
        testCandidate.Complete(IsSectionCompleted.No);

        // then
        testCandidate.Status.Should().Be(SiteStatus.InProgress);
    }

    private static SiteEntityBuilder CreateAnsweredSiteBuilder(SiteStatus status = SiteStatus.InProgress)
    {
        return SiteEntityBuilder.New()
            .WithStatus(status)
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
            .WithModernMethodsOfConstruction(SiteModernMethodsOfConstructionBuilder.New()
                .WithIsUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
                .WithInformation("some barriers", "some impact")
                .Build())
            .WithProcurements(SiteProcurementsBuilder.New().WithProcurements(SiteProcurement.PartneringSupplyChain).Build());
    }
}
