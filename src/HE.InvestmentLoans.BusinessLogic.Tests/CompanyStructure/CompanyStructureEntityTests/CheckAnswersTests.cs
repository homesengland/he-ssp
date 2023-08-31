using FluentAssertions;
using HE.InvestmentLoans.BusinessLogic.Tests.Assertions;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestDataBuilders;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Application.Enums;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.CompanyStructureEntityTests;

public class CheckAnswersTests
{
    [Fact]
    public void ShouldThrowValidationException_WhenCheckAnswersIsYesButAllAnswersAreNotProvided()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestObjectBuilder.New().Build();

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
        var companyStructureEntity = CompanyStructureEntityTestObjectBuilder
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
        var companyStructureEntity = CompanyStructureEntityTestObjectBuilder.New().Build();

        // when
        var action = () => companyStructureEntity.CheckAnswers(YesNoAnswers.Undefined);

        // then
        action.Should().ThrowExactly<DomainValidationException>().WithOnlyOneErrorMessage(ValidationErrorMessage.SecurityCheckAnswers);
        companyStructureEntity.Status.Should().NotBe(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldNotThrowValidationException_WhenCheckAnswersIsNoAndSomeAnswersAreNotProvided()
    {
        // given
        var companyStructureEntity = CompanyStructureEntityTestObjectBuilder.New().WithHomesBuild().Build();

        // when
        var action = () => companyStructureEntity.CheckAnswers(YesNoAnswers.No);

        // then
        action.Should().NotThrow<DomainValidationException>();
        companyStructureEntity.Status.Should().Be(SectionStatus.InProgress);
    }
}
