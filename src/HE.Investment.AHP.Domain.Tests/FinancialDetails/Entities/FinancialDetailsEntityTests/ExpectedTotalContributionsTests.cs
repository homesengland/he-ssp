using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.FinancialDetailsEntityTests;

public class ExpectedTotalContributionsTests
{
    [Fact]
    public void ShouldReturnZero_WhenAllContributionsAreNotProvided()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .Build();

        // when
        var expectedTotalContributions = financialDetailsEntity.ExpectedTotalContributions();

        // then
        expectedTotalContributions.Should().Be(0);
    }

    [Fact]
    public void ShouldReturnExpectedTotalContributions()
    {
        // given
        var expectedContributionsToScheme = ExpectedContributionsToSchemeBuilder
            .New()
            .WithOwnResources(200)
            .Build();

        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithExpectedContributions(expectedContributionsToScheme)
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalContributions();

        // then
        expectedTotalCosts.Should().Be(200);
    }

    [Fact]
    public void ShouldReturnExpectedTotalContributions_WhenContributionsAndGrantsAreProvided()
    {
        // given
        var expectedContributionsToScheme = ExpectedContributionsToSchemeBuilder
            .New()
            .WithOwnResources(200)
            .WithSharedOwnershipSales(200)
            .Build();

        var publicGrants = PublicGrantsBuilder
            .New()
            .WithHealthRelatedGrants(200)
            .Build();

        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithExpectedContributions(expectedContributionsToScheme)
            .WithGrants(publicGrants)
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalContributions();

        // then
        expectedTotalCosts.Should().Be(600);
    }

    [Fact]
    public void ShouldReturnExpectedTotalContributionsWithoutSharedOwnershipSales_WhenTenureIsNotSharedOwnership()
    {
        // given
        var expectedContributionsToScheme = ExpectedContributionsToSchemeBuilder
            .New()
            .WithOwnResources(200)
            .WithSharedOwnershipSales(100)
            .Build();

        var publicGrants = PublicGrantsBuilder
            .New()
            .WithHealthRelatedGrants(200)
            .Build();

        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithExpectedContributions(expectedContributionsToScheme)
            .WithGrants(publicGrants)
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalContributions();

        // then
        expectedTotalCosts.Should().Be(400);
    }
}
