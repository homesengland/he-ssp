using FluentAssertions;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Scheme.Entities.SchemeEntityTests;

public class CompleteTests
{
    [Fact]
    public void ShouldThrowException_WhenNotAnswersAreProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithHousingNeeds(new HousingNeeds(null, null)).Build();

        // when
        var complete = () => testCandidate.Complete();

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(2);
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenAnswerIsYesAndAllQuestionsAreAnswered()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenTenureIsAffordableRentAndAffordabilityAndSalesRiskAreNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder()
            .WithApplicationDetails(ApplicationBasicInfoTestData.AffordableRentInDraftState)
            .WithAffordabilityEvidence(new AffordabilityEvidence(null))
            .WithSalesRisk(new SalesRisk(null))
            .Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenTenureIsSharedOwnershipAndAffordabilityAndSalesRiskAreProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder().WithApplicationDetails(ApplicationBasicInfoTestData.SharedOwnershipInDraftState).Build();

        // when
        testCandidate.Complete();

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldThrowException_WhenTenureIsSharedOwnershipAndAffordabilityIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder()
            .WithApplicationDetails(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithAffordabilityEvidence(new AffordabilityEvidence(null))
            .Build();

        // when
        var complete = () => testCandidate.Complete();

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void ShouldThrowException_WhenTenureIsSharedOwnershipAndSalesRiskIsNotProvided()
    {
        // given
        var testCandidate = CreateAnsweredSiteBuilder()
            .WithApplicationDetails(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithSalesRisk(new SalesRisk(null))
            .Build();

        // when
        var complete = () => testCandidate.Complete();

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
    }

    private static SchemeEntityBuilder CreateAnsweredSiteBuilder() =>
        SchemeEntityBuilder.NewNotStarted()
            .WithApplicationDetails(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithSectionStatus(SectionStatus.InProgress)
            .WithSchemeFunding(new SchemeFunding("100000", "100"))
            .WithAffordabilityEvidence(new AffordabilityEvidence("evidence"))
            .WithSalesRisk(new SalesRisk("risk"))
            .WithHousingNeeds(new HousingNeeds("housing", "needs"));
}
