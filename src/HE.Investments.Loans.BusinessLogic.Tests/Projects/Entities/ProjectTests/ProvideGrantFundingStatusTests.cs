using HE.Investments.Common.Contract;
using HE.Investments.Loans.BusinessLogic.Projects.Entities;
using HE.Investments.Loans.BusinessLogic.Tests.Projects.TestData;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
public class ProvideGrantFundingStatusTests
{
    [Theory]
    [InlineData(PublicSectorGrantFundingStatus.NotReceived)]
    [InlineData(PublicSectorGrantFundingStatus.Unknown)]
    [InlineData(PublicSectorGrantFundingStatus.Received)]
    public void ShouldSetProvidedStatus(PublicSectorGrantFundingStatus status)
    {
        // given
        var project = new Project();

        // when
        project.ProvideGrantFundingStatus(status);

        // then
        project.GrantFundingStatus.Should().Be(status);
    }

    [Theory]
    [InlineData(PublicSectorGrantFundingStatus.NotReceived)]
    [InlineData(PublicSectorGrantFundingStatus.Unknown)]
    public void ShouldRemoveGrantInformation_WhenStatusChangesFromReceivedToAnyOther(PublicSectorGrantFundingStatus status)
    {
        // given
        var project = new Project();

        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);
        project.ProvideGrantFundingInformation(PublicSectorGrantFundingTestData.AnyGrantFunding);

        // when
        project.ProvideGrantFundingStatus(status);

        // then
        project.PublicSectorGrantFunding.Should().BeNull();
    }

    [Fact]
    public void ShouldChangeStatusToInProgress()
    {
        // given
        var project = new Project();

        // when
        project.ProvideGrantFundingStatus(PublicSectorGrantFundingStatus.Received);

        // then
        project.Status.Should().Be(SectionStatus.InProgress);
    }
}
