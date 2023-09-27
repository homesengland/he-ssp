using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans;

[Order(0)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class CompleteUserProfileIntegrationTests : IntegrationTest
{
    public CompleteUserProfileIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        if (string.IsNullOrWhiteSpace(UserData.UserGlobalId) && !UserData.IsDeveloperProvidedUserData)
        {
            UserData.ProvideData($"itests|{Guid.NewGuid()}");
            TestClient.AsLoggedUser();
        }

        Skip.If(UserData.IsDeveloperProvidedUserData, "Developer provided own user which has completed profile and those tests cannot be run");
    }

    [SkippableFact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToCompleteUserProfile_WhenUserIsLoggedInButProfileIsNotCompleted()
    {
        // given & when
        var completeProfilePage = await TestClient
            .NavigateTo(PagesUrls.MainPage);

        // then
        completeProfilePage
            .UrlEndWith(OrganizationPagesUrls.CompleteProfileDetails)
            .HasTitle("Complete your details");

        SetSharedData(SharedKeys.CurrentPageKey, completeProfilePage);
    }

    [SkippableFact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldRedirectToDashboardPage_WhenUserIsLoggedInAndProfileIsCompleted()
    {
        // given
        var completeProfilePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        completeProfilePage.HasGdsSubmitButton("save-and-continue-button", out var saveAndContinueButton);
        completeProfilePage = await TestClient.SubmitButton(saveAndContinueButton);

        // then
        completeProfilePage
            .UrlEndWith(OrganizationPagesUrls.CompleteProfileDetails)
            .HasTitle("Complete your details")
            .HasValidationMessages(
                "Enter your job title",
                "Enter your last name",
                "Enter your first name",
                "Enter your preferred telephone number");
    }

    [SkippableFact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateToOrganisationPage_WhenUserCompletedProfile()
    {
        // given
        var completeProfilePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        completeProfilePage.HasGdsSubmitButton("save-and-continue-button", out var saveAndContinueButton);
        var organizationSearchPage = await TestClient.SubmitButton(saveAndContinueButton, new Dictionary<string, string>
        {
            { "FirstName", UserData.FirstName },
            { "LastName", UserData.LastName },
            { "JobTitle", "AI Tester" },
            { "TelephoneNumber", UserData.TelephoneNumber },
        });

        // then
        organizationSearchPage
            .UrlEndWith(OrganizationPagesUrls.OrganizationSearch)
            .HasTitle("Search for your organisation");

        SetSharedData(SharedKeys.CurrentPageKey, organizationSearchPage);
    }

    [SkippableFact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToOrganisationSearch_WhenUserCompletedProfileButNotOrganisationIsNotLinked()
    {
        // given & when
        var organizationSearchPage = await TestClient.NavigateTo(PagesUrls.MainPage);

        // then
        organizationSearchPage
            .UrlEndWith(OrganizationPagesUrls.OrganizationSearch)
            .HasTitle("Search for your organisation");

        SetSharedData(SharedKeys.CurrentPageKey, organizationSearchPage);
    }

    [SkippableFact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order06_ShouldSearchOrganization()
    {
        // given
        var organizationSearchPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        organizationSearchPage.HasGdsSubmitButton("search-organization-button", out var searchButton);
        var organizationSearchResultPage = await TestClient.SubmitButton(searchButton, new Dictionary<string, string>
        {
            { "Name", UserData.OrganizationName },
        });

        // then
        organizationSearchResultPage
            .UrlWithoutQueryEndsWith(OrganizationPagesUrls.SearchOrganizationResult)
            .HasTitle("Select your organisation");

        SetSharedData(SharedKeys.CurrentPageKey, organizationSearchResultPage);
    }

    [SkippableFact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order07_ShouldSelectOrganizationAndNavigateToOrganizationDashboardPage()
    {
        // given
        var organizationSearchResultPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        organizationSearchResultPage.HasAnchor($"company-link-{UserData.OrganizationRegistrationNumber}", out var selectOrganizationLink);
        var dashboardPage = await TestClient.NavigateTo(selectOrganizationLink);

        // then
        dashboardPage
            .UrlEndWith(PagesUrls.DashboardPage)
            .HasTitle($"{UserData.OrganizationName} home page");
    }
}
