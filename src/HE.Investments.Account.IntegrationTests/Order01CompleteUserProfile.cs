using System.Diagnostics.CodeAnalysis;
using HE.Investments.Account.IntegrationTests.Extensions;
using HE.Investments.Account.IntegrationTests.Framework;
using HE.Investments.Account.IntegrationTests.Pages;
using HE.Investments.Account.WWW;
using HE.Investments.Account.WWW.Views.Organisation;
using HE.Investments.Account.WWW.Views.User;
using HE.Investments.Account.WWW.Views.UserOrganisations.Const;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Account.IntegrationTests;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01CompleteUserProfile : AccountIntegrationTest
{
    public Order01CompleteUserProfile(IntegrationTestFixture<Program> fixture)
        : base(fixture, true)
    {
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToCompleteUserProfile_WhenProfileIsNotCompleted()
    {
        // given & when
        var profileDetailsPage = await TestClient
            .NavigateTo(MainPagesUrl.MainPage);

        // then
        profileDetailsPage
            .UrlEndWith(MainPagesUrl.ProfileDetails)
            .HasTitle(UserPageTitles.ProfileDetails);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayErrorMessages_WhenUserDoesNotProvideProfileDetails()
    {
        // given
        var currentPage = await GetCurrentPage(MainPagesUrl.ProfileDetails);
        currentPage
            .UrlWithoutQueryEndsWith(MainPagesUrl.ProfileDetails)
            .HasTitle(UserPageTitles.ProfileDetails)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var profileDetailsPage = await TestClient.SubmitButton(continueButton);

        // then
        profileDetailsPage.HasValidationMessages(
            "Enter your job title",
            "Enter your last name",
            "Enter your first name",
            "Enter your preferred telephone number");
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateToOrganisationPage_WhenUserCompletedProfile()
    {
        // given
        var profileDetailsPage = await GetCurrentPage(MainPagesUrl.ProfileDetails);
        profileDetailsPage.UrlEndWith(MainPagesUrl.ProfileDetails)
            .HasTitle(UserPageTitles.ProfileDetails)
            .HasSaveAndContinueButton(out var continueButton);
        var profileData = FreshProfileData.GenerateProfileData();

        // when
        var organisationSearchPage = await TestClient.SubmitButton(
            continueButton,
            ("FirstName", profileData.FirstName),
            ("LastName", profileData.LastName),
            ("JobTitle", profileData.JobTitle),
            ("TelephoneNumber", profileData.TelephoneNumber));

        // then
        organisationSearchPage.UrlEndWith(OrganisationPagesUrls.Search)
            .HasTitle(OrganisationPageTitles.SearchForYourOrganisation);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToOrganisationSearch_WhenUserCompletedProfileButNotOrganisationIsNotLinked()
    {
        // given & when
        var organisationSearchPage = await TestClient.NavigateTo(MainPagesUrl.MainPage);

        // then
        organisationSearchPage
            .UrlEndWith(OrganisationPagesUrls.Search)
            .HasTitle(OrganisationPageTitles.SearchForYourOrganisation);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldSearchOrganisation()
    {
        // given
        var profileData = FreshProfileData.GenerateOrganisationSearch();
        var organisationSearchPage = await GetCurrentPage(OrganisationPagesUrls.Search);
        organisationSearchPage
            .UrlEndWith(OrganisationPagesUrls.Search)
            .HasTitle(OrganisationPageTitles.SearchForYourOrganisation)
            .HasSubmitButton(out var searchButton, "Search");

        // when
        var organisationSearchResultPage = await TestClient.SubmitButton(searchButton, ("Name", profileData.OrganisationName));

        // then
        organisationSearchResultPage
            .UrlEndWith(OrganisationPagesUrls.SearchResult(profileData.OrganisationName))
            .HasTitle(OrganisationPageTitles.SelectYourOrganisation);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldSelectOrganisationAndNavigateToConfirmPage()
    {
        // given
        var organisationSearchResultPage = await GetCurrentPage(OrganisationPagesUrls.SearchResult(FreshProfileData.OrganisationName));
        organisationSearchResultPage
            .UrlEndWith(OrganisationPagesUrls.SearchResult(FreshProfileData.OrganisationName))
            .HasTitle(OrganisationPageTitles.SelectYourOrganisation)
            .HasOrganisationSearchResultItem(FreshProfileData.OrganisationName, out var organisationId)
            .HasLinkWithTestId($"confirm-organisation-{organisationId}", out var confirmOrganisationLink);

        // when
        var confirmationPage = await TestClient.NavigateTo(confirmOrganisationLink);

        // then
        confirmationPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Confirm(organisationId))
            .HasTitle(OrganisationPageTitles.ConfirmYourSelection);
        FreshProfileData.SetSelectedOrganisationId(organisationId);
        UserOrganisationsData.SetOrganisationId(organisationId);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldConfirmOrganisationAndNavigateToDashboardPage()
    {
        // given
        var confirmationPage = await GetCurrentPage(OrganisationPagesUrls.Confirm(FreshProfileData.SelectedOrganisationId));
        confirmationPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Confirm(FreshProfileData.SelectedOrganisationId))
            .HasTitle(OrganisationPageTitles.ConfirmYourSelection)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        // then
        var organisationsList = await TestClient.SubmitButton(continueButton, ("IsConfirmed", "True"));

        // then
        organisationsList
            .UrlEndWith(MainPagesUrl.OrganisationList)
            .HasTitle(UserOrganisationsPageTitles.List)
            .HasLinkWithTestId("add-another-organisation-link", out _)
            .HasLinkWithTestId("manage-profile-link", out _);

        SaveCurrentPage();
    }
}
