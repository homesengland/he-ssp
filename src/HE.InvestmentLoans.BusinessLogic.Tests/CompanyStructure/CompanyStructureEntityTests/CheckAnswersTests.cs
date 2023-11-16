using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.CompanyStructureEntityTests;

public class CheckAnswersTests
{
    [Fact]
    public void ShouldThrowValidationException_WhenCheckAnswersIsYesButAllAnswersAreNotProvided()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestBuilder.New().Build();

        // when
        var action = () => companyStructureEntity.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.CheckAnswersOption);
        companyStructureEntity.Status.Should().NotBe(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldCompleteSection_WhenCheckAnswersIsYesAndAllAnswersAreProvided()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestBuilder
            .New()
            .WithHomesBuild()
            .WithCompanyPurpose()
            .WithMoreInformation()
            .Build();

        // when
        var action = () => companyStructureEntity.CheckAnswers(YesNoAnswers.Yes);

        // then
        action.Should().NotThrow<DomainValidationException>();
        companyStructureEntity.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldThrowValidationException_WhenCheckAnswersIsNotProvided()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestBuilder.New().Build();

        // when
        var action = () => companyStructureEntity.CheckAnswers(YesNoAnswers.Undefined);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.NoCheckAnswers);
        companyStructureEntity.Status.Should().NotBe(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldNotThrowValidationException_WhenCheckAnswersIsNoAndSomeAnswersAreNotProvided()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestBuilder.New().WithHomesBuild().Build();

        // when
        var action = () => companyStructureEntity.CheckAnswers(YesNoAnswers.No);

        // then
        action.Should().NotThrow<DomainValidationException>();
        companyStructureEntity.Status.Should().Be(SectionStatus.InProgress);
    }
}
