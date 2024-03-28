using FluentAssertions;
using HE.Investments.FrontDoor.WWW.Helpers;
using Xunit;

namespace HE.Investments.FrontDoor.WWW.Tests.HelpersTests.SiteIdFromActionUrlHelperTests;

public class GetTests
{
    [Fact]
    public void ShouldReturnSiteId_WhenActionUrlIsProvided()
    {
        // given
        var siteId = "dad5bdc3-69e5-ee11-904c-000d3a86ce60";
        var actionUrl = $"/apply-for-support/project/0aa1acb8-aee3-ee11-904c-0022481b49d4/site/{siteId}/planning-status?redirect=CheckAnswers";

        // when
        var result = SiteIdFromActionUrlHelper.Get(actionUrl);

        // then
        result.Should().Be(siteId);
    }
}
