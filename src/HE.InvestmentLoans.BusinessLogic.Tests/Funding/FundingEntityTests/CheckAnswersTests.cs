using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Funding.TestObjectBuilders;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Funding.FundingEntityTests;

public class CheckAnswersTests
{
    [Fact]
    public void ShouldThrowValidationException_WhenCheckAnswersIsYesButAllAnswersAreNotProvided()
    {
        // given
        var fundingEntity = FundingEntityTestBuilder.New().Build();

        // when
        var action = () => fundingEntity.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
        fundingEntity.Status.Should().NotBe(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldCompleteSection_WhenCheckAnswersIsYesAndAllAnswersAreProvided()
    {
        // given
        var fundingEntity = FundingEntityTestBuilder
            .New()
            .WithAllDataProvided()
            .Build();

        // when
        var action = () => fundingEntity.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().NotThrow<DomainValidationException>();
        fundingEntity.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldThrowValidationException_WhenCheckAnswersIsNotProvided()
    {
        // given
        var fundingEntity = CompanyStructureEntityTestBuilder.New().Build();

        // when
        var action = () => fundingEntity.CheckAnswers(YesNoAnswers.Undefined);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoCheckAnswers);
        fundingEntity.Status.Should().NotBe(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldNotThrowValidationException_WhenCheckAnswersIsNoAndSomeAnswersAreNotProvided()
    {
        // given
        var fundingEntity = FundingEntityTestBuilder.New().WithAbnormalCosts().WithGrossDevelopmentValue().Build();

        // when
        var action = () => fundingEntity.CheckAnswers(YesNoAnswers.No);

        // then
        action.Should().NotThrow<DomainValidationException>();
        fundingEntity.Status.Should().Be(SectionStatus.InProgress);
    }
}
