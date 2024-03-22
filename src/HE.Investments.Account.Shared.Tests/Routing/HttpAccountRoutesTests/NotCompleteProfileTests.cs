using FluentAssertions;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HE.Investments.Account.Shared.Tests.Routing.HttpAccountRoutesTests;

public class NotCompleteProfileTests : TestBase<HttpAccountRoutes>
{
    [Fact]
    public void ShouldReturnRedirectResult_WhenCallbackIsNotProvided()
    {
        // given
        RegisterConfig("account.com");

        // when
        var result = TestCandidate.NotCompleteProfile();

        // then
        result.Should().BeOfType<RedirectResult>();
        ((RedirectResult)result).Url.Should().Be("account.com/user/profile-details");
    }

    [Fact]
    public void ShouldReturnRedirectResult_WhenOnlyCallbackRouteIsProvided()
    {
        // given
        RegisterConfig("account.com");

        // when
        var result = TestCandidate.NotCompleteProfile(callbackRoute: "my/route");

        // then
        result.Should().BeOfType<RedirectResult>();
        ((RedirectResult)result).Url.Should().Be("account.com/user/profile-details?callback=my/route");
    }

    [Fact]
    public void ShouldReturnRedirectResult_WhenCallbackProgrammeAndRouteIsProvided()
    {
        // given
        RegisterConfig("account.com");

        // when
        var result = TestCandidate.NotCompleteProfile(callbackProgramme: "my-programme", callbackRoute: "my/route");

        // then
        result.Should().BeOfType<RedirectResult>();
        ((RedirectResult)result).Url.Should().Be("account.com/user/profile-details?programme=my-programme&callback=my/route");
    }

    private void RegisterConfig(string url)
    {
        RegisterDependency(new AccountConfig { Url = url });
    }
}
