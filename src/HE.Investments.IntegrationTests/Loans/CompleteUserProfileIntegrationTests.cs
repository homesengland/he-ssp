using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Organization;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans;

[Order(0)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class CompleteUserProfileIntegrationTests : IntegrationTest
{
    public CompleteUserProfileIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        if (string.IsNullOrWhiteSpace(LoginData.UserGlobalId) && !UserData.IsDeveloperProvidedUserData)
        {
            ProvideLoginData($"itests|{Guid.NewGuid()}");
            TestClient.AsLoggedUser();
        }

        Skip.If(UserData.IsDeveloperProvidedUserData, "Developer provided own user which has completed profile and those tests cannot be run");
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(1)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
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

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(2)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
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

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(3)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task Order03_ShouldNavigateToOrganisationPage_WhenUserCompletedProfile()
    {
        // given
        var completeProfilePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        completeProfilePage.HasGdsSubmitButton("save-and-continue-button", out var saveAndContinueButton);
        var organizationSearchPage = await TestClient.SubmitButton(saveAndContinueButton, new Dictionary<string, string>
        {
            { "FirstName", UserData.FirstName.ToString() },
            { "LastName", UserData.LastName.ToString() },
            { "JobTitle", "AI Tester" },
            { "TelephoneNumber", UserData.TelephoneNumber.ToString() },
        });

        // then
        organizationSearchPage
            .UrlEndWith(OrganizationPagesUrls.OrganizationSearch)
            .HasTitle(OrganizationPageTitles.SearchForYourOrganisation);

        SetSharedData(SharedKeys.CurrentPageKey, organizationSearchPage);
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(4)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task Order04_ShouldRedirectToOrganisationSearch_WhenUserCompletedProfileButNotOrganisationIsNotLinked()
    {
        // given & when
        var organizationSearchPage = await TestClient.NavigateTo(PagesUrls.MainPage);

        // then
        organizationSearchPage
            .UrlEndWith(OrganizationPagesUrls.OrganizationSearch)
            .HasTitle(OrganizationPageTitles.SearchForYourOrganisation);

        SetSharedData(SharedKeys.CurrentPageKey, organizationSearchPage);
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(5)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task Order05_ShouldSearchOrganization()
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
            .HasTitle(OrganizationPageTitles.SelectYourOrganisation);

        SetSharedData(SharedKeys.CurrentPageKey, organizationSearchResultPage);
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(6)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task Order06_ShouldSelectOrganizationAndNavigateToConfirmPage()
    {
        // given
        var organizationSearchResultPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        organizationSearchResultPage.HasAnchor($"company-link-{UserData.OrganizationRegistrationNumber}", out var selectOrganizationLink);
        var confirmationPage = await TestClient.NavigateTo(selectOrganizationLink);

        // then
        confirmationPage
            .UrlEndWith(OrganizationPagesUrls.ConfirmOrganization(UserData.OrganizationRegistrationNumber))
            .HasTitle(OrganizationPageTitles.ConfirmYourSelection);

        SetSharedData(SharedKeys.CurrentPageKey, confirmationPage);
    }

    [SkippableFact(typeof(SkipException), Skip = LoansConfig.SkipTest)]
    [Order(7)]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task Order07_ShouldConfirmOrganizationAndNavigateToDashboardPage()
    {
        // given
        var confirmationPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        confirmationPage.HasGdsSubmitButton("continue-button", out var continueButton);
        var dashboardPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "Response", CommonResponse.Yes } });

        // then
        dashboardPage
            .UrlEndWith(PagesUrls.DashboardPage)
            .HasTitle($"{UserData.OrganizationName} LUHBF applications");
    }
}
