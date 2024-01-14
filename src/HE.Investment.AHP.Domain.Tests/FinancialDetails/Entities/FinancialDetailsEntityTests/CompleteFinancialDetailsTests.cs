using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.FluentAssertions;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.FinancialDetailsEntityTests;

public class CompleteFinancialDetailsTests
{
    [Fact]
    public void ShouldChangeSectionStatusToCompleted_WhenAllSectionsAreCompleted()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithLandStatus(10)
            .WithLandValue(700)
            .WithOtherApplicationCosts(25, 25)
            .WithExpectedContributions(ExpectedContributionsToSchemeBuilder.NewWithAllValuesAs50().Build())
            .WithGrants(PublicGrantsBuilder.NewWithAllValuesAs50().Build())
            .Build();

        // when
        financialDetailsEntity.CompleteFinancialDetails(IsSectionCompleted.Yes);

        // then
        financialDetailsEntity.SectionStatus.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenSectionIsNotCompleted()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithLandStatus(10)
            .Build();

        // when
        var action = () => financialDetailsEntity.CompleteFinancialDetails(IsSectionCompleted.Yes);

        // then
        action.Should().Throw<DomainValidationException>().WithSingleError(ValidationErrorMessage.SectionIsNotCompleted);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenSectionIsCompletedIsUndefined()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithLandStatus(10)
            .Build();

        // when
        var action = () => financialDetailsEntity.CompleteFinancialDetails(IsSectionCompleted.Undefied);

        // then
        action.Should().Throw<DomainValidationException>().WithSingleError("Select whether you have completed this section");
    }

    [Fact]
    public void ShouldChangeStatusSectionToInProgress_WhenSectionIsCompletedIsNo()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithLandStatus(10)
            .Build();

        // when
        financialDetailsEntity.CompleteFinancialDetails(IsSectionCompleted.No);

        // then
        financialDetailsEntity.SectionStatus.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public void ShouldThrowDomainValidationException_WhenCostAreNotTheSameAsContributions()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New(ApplicationBasicInfoTestData.SharedOwnershipInDraftState)
            .WithLandStatus(10)
            .WithLandValue(700)
            .WithOtherApplicationCosts(25, 20)
            .WithExpectedContributions(ExpectedContributionsToSchemeBuilder.NewWithAllValuesAs50().Build())
            .WithGrants(PublicGrantsBuilder.NewWithAllValuesAs50().Build())
            .Build();

        // when
        var action = () => financialDetailsEntity.CompleteFinancialDetails(IsSectionCompleted.Yes);

        // then
        action.Should().Throw<DomainValidationException>().WithSingleError(FinancialDetailsValidationErrors.CostsAndFundingMismatch);
    }
}
