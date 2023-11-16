using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Common;
using HE.Investments.Common.Domain;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvideMoreInformationTests
{
    [Theory]
    [InlineData(PublicSectorGrantFundingStatus.NotReceived)]
    [InlineData(PublicSectorGrantFundingStatus.Unknown)]
    public void ShouldFail_WhenGrantWasNotReceived(PublicSectorGrantFundingStatus status)
    {
        // given
        var funding = new Project();

        funding.ProvideGrantFundingStatus(status);

        // when
        var action = () => funding.ProvideGrantFundingInformation(new PublicSectorGrantFunding(new ShortText("text"), new Pounds(1), new ShortText("text"), new LongText("text")));

        // then
        action.Should().ThrowExactly<DomainException>();
    }

    [Fact]
    public void ShouldNotFail_WhenGrantWasReceived()
    {
        // given
        var project = new Project();

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        // when
        var action = () => project.ProvideGrantFundingInformation(new PublicSectorGrantFunding(new ShortText("text"), new Pounds(1), new ShortText("text"), new LongText("text")));

        // then
        action.Should().NotThrow<DomainException>();
    }

    [Fact]
    public void ShouldSetAdditionalInformation_WhenGrantWasReceived()
    {
        // given
        var project = new Project();

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        // when
        project.ProvideGrantFundingInformation(new PublicSectorGrantFunding(new ShortText("text"), new Pounds(1), new ShortText("text"), new LongText("text")));

        // then
        project.PublicSectorGrantFunding.Should().NotBeNull();

        project.PublicSectorGrantFunding!.ProviderName.Should().Be(new ShortText("text"));
        project.PublicSectorGrantFunding!.Amount.Should().Be(new Pounds(1));
        project.PublicSectorGrantFunding!.GrantOrFundName.Should().Be(new ShortText("text"));
        project.PublicSectorGrantFunding!.Purpose.Should().Be(new LongText("text"));
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        // when
        project.ProvideGrantFundingInformation(new PublicSectorGrantFunding(new ShortText("text"), new Pounds(1), new ShortText("text"), new LongText("text")));

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
