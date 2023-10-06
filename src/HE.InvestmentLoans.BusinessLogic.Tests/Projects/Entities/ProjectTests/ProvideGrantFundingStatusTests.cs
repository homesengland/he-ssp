using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Generic;
using HE.InvestmentLoans.BusinessLogic.Projects.Entities;
using HE.InvestmentLoans.BusinessLogic.Tests.Projects.TestData;
using HE.InvestmentLoans.Common.Exceptions;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Projects.Entities.ProjectTests;
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
}
