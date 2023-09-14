using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans;

[Order(0)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class GuidanceIntegrationTests : IntegrationTest
{
    private const string CurrentPageKey = "CurrentPage";

    public GuidanceIntegrationTests(IntegrationTestFixture<Program> fixture)
     : base(fixture)
    {
    }

    [Fact(Skip = "LoansConfig.SkipTest")]
    [Order(1)]
    public async Task Order01_ShouldRedirectToGuidancePage_WhenUserIsNotLogged()
    {
        // given & when
        var mainPage = await TestClient.AsNotLoggedUser().NavigateTo(PagesUrls.MainPage);

        // then
        mainPage
            .UrlEndWith(GuidancePagesUrls.WhatTheHomeBuildingFundIs)
            .HasGdsButton("SignInButton");
        SetSharedData(CurrentPageKey, mainPage);
    }

    [Fact(Skip = "LoansConfig.SkipTest")]
    [Order(2)]
    public async Task Order02_ShouldRedirectFromWhatTheHomeBuildingFundIsToEligibilityPage()
    {
        // given
        var mainPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var nextLink = mainPage.GetAnchorElementById("guidance-next-link");
        var eligibilityPage = await TestClient.AsNotLoggedUser().NavigateTo(nextLink);

        // then
        eligibilityPage
            .UrlEndWith(GuidancePagesUrls.Eligibility)
            .HasGdsButton("SignInButton");

        SetSharedData(CurrentPageKey, eligibilityPage);
    }

    [Fact(Skip = "LoansConfig.SkipTest")]
    [Order(3)]
    public async Task Order03_ShouldRedirectFromEligibilityPageToApplyPageAndApplyLinkShouldBeVisible()
    {
        // given
        var eligibilityPage = GetSharedData<IHtmlDocument>(CurrentPageKey);

        // when
        var nextLink = eligibilityPage.GetAnchorElementById("guidance-next-link");
        var applyPage = await TestClient.AsNotLoggedUser().NavigateTo(nextLink);

        // then
        applyPage
            .UrlEndWith(GuidancePagesUrls.Apply)
            .GetAnchorElementById("apply-link")
            .PathName.Should()
            .EndWith("/application");
    }
}
