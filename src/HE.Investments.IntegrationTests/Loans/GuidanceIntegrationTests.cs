using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Assertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans;

[Order(0)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class GuidanceIntegrationTests : IntegrationTest
{
    // Make it null when you want to run tests locally
    private const string? Skip = "Waits for DevOps configuration - #76791";

    private const string CurrentPageKey = "CurrentPage";

    public GuidanceIntegrationTests(IntegrationTestFixture<Program> fixture)
     : base(fixture)
    {
    }

    [Fact(Skip = Skip)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToGuidancePage_WhenUserIsNotLogged()
    {
        // given & when
        var mainPage = await TestClient.AsNotLoggedUser().NavigateTo(PagesUrls.MainPage);

        // then
        mainPage.Url.Should().EndWith(GuidancePagesUrls.WhatTheHomeBuildingFundIs);
        mainPage.GetElementById("SignInButton").Should().BeGdsButton();
        SetSharedData(CurrentPageKey, mainPage);
    }

    [Fact(Skip = Skip)]
    [Order(2)]
    public async Task Order02_ShouldRedirectFromWhatTheHomeBuildingFundIsToEligibilityPage()
    {
        // given
        var currentPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var nextLink = currentPage.GetAnchorElementById("guidance-next-link");
        var eligibilityPage = await TestClient.AsNotLoggedUser().ClickAHrefElement(nextLink);

        // then
        eligibilityPage.Url.Should().EndWith(GuidancePagesUrls.Eligibility);
        eligibilityPage.GetElementById("SignInButton").Should().BeGdsButton();
        SetSharedData(CurrentPageKey, eligibilityPage);
    }

    [Fact(Skip = Skip)]
    [Order(3)]
    public async Task Order03_ShouldRedirectFromEligibilityPageToApplyPageAndApplyLinkShouldBeVisible()
    {
        // given
        var currentPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var nextLink = currentPage.GetAnchorElementById("guidance-next-link");
        var applyPage = await TestClient.AsNotLoggedUser().ClickAHrefElement(nextLink);

        // then
        applyPage.Url.Should().EndWith(GuidancePagesUrls.Apply);
        applyPage.GetAnchorElementById("apply-link").PathName.Should().EndWith("/application");
    }
}
