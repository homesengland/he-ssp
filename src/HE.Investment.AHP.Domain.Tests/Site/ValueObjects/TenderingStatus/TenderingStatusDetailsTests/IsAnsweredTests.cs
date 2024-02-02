using FluentAssertions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.TenderingStatus.TenderingStatusDetailsTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenTenderingStatusNotApplicable()
    {
        // given
        var details = new TenderingStatusDetails(SiteTenderingStatus.NotApplicable);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(SiteTenderingStatus.UnconditionalWorksContract)]
    [InlineData(SiteTenderingStatus.ConditionalWorksContract)]
    public void ShouldReturnTrue_WhenTenderingStatusUnconditionalOrConditionalWorksContract(SiteTenderingStatus status)
    {
        // given
        var details = new TenderingStatusDetails(status, new ContractorName("szwagier"), true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenStatusNotProvided()
    {
        // given
        var details = new TenderingStatusDetails();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(SiteTenderingStatus.UnconditionalWorksContract)]
    [InlineData(SiteTenderingStatus.ConditionalWorksContract)]
    public void ShouldReturnFalse_WhenContractorNameNotProvided(SiteTenderingStatus status)
    {
        // given
        var details = new TenderingStatusDetails(status, null, true);

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(SiteTenderingStatus.UnconditionalWorksContract)]
    [InlineData(SiteTenderingStatus.ConditionalWorksContract)]
    public void ShouldReturnFalse_WhenIsSmeNotProvided(SiteTenderingStatus status)
    {
        // given
        var details = new TenderingStatusDetails(status, new ContractorName("szwagier"));

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
