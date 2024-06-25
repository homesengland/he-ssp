using System.Diagnostics.CodeAnalysis;
using HE.Investments.Account.IntegrationTests.Extensions;
using HE.Investments.Account.IntegrationTests.Framework;
using HE.Investments.Account.IntegrationTests.Pages;
using HE.Investments.Account.WWW;
using HE.Investments.Account.WWW.Views.Organisation;
using HE.Investments.Account.WWW.Views.UserOrganisations.Const;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Account.IntegrationTests;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03MultipleOrganisations : AccountIntegrationTest
{
    public Order03MultipleOrganisations(IntegrationTestFixture<Program> fixture)
        : base(fixture, true)
    {
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToSearchOrganisations_WhenUserSelectsAddAnotherOrganisationLink()
    {
        // given
        var organisationsList = await TestClient
            .NavigateTo(MainPagesUrl.OrganisationList);
        organisationsList
            .UrlEndWith(MainPagesUrl.OrganisationList)
            .HasTitle(UserOrganisationsPageTitles.List)
            .HasTotalSummaryCards(1)
            .HasSummaryCardWithTitle(FreshProfileData.OrganisationName)
            .HasLinkWithTestId("add-another-organisation-link", out var addAnotherOrganisationLink)
            .HasLinkWithTestId("manage-profile-link", out _);

        // when
        var organisationSearchPage = await TestClient.NavigateTo(addAnotherOrganisationLink);

        // then
        organisationSearchPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Search)
            .HasTitle(OrganisationPageTitles.SearchForYourOrganisation);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldSearchSecondOrganisation()
    {
        // given
        UserOrganisationsData.SetSecondOrganisationName("HOMES OF ENGLAND FIRST CHARGE LTD");
        var organisationSearchPage = await GetCurrentPage(OrganisationPagesUrls.Search);
        organisationSearchPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Search)
            .HasTitle(OrganisationPageTitles.SearchForYourOrganisation)
            .HasSubmitButton(out var searchButton, "Search");

        // when
        var organisationSearchResultPage = await TestClient.SubmitButton(searchButton, ("Name", UserOrganisationsData.SecondaryOrganisationName));

        // then
        organisationSearchResultPage
            .UrlEndWith(OrganisationPagesUrls.SearchResult(UserOrganisationsData.SecondaryOrganisationName))
            .HasTitle(OrganisationPageTitles.SelectYourOrganisation);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldSelectSecondOrganisationAndNavigateToConfirmPage()
    {
        // given
        var organisationSearchResultPage = await GetCurrentPage(OrganisationPagesUrls.SearchResult(UserOrganisationsData.SecondaryOrganisationName));
        organisationSearchResultPage
            .UrlEndWith(OrganisationPagesUrls.SearchResult(UserOrganisationsData.SecondaryOrganisationName))
            .HasTitle(OrganisationPageTitles.SelectYourOrganisation)
            .HasOrganisationSearchResultItem(UserOrganisationsData.SecondaryOrganisationName, out var organisationId)
            .HasLinkWithTestId($"confirm-organisation-{organisationId}", out var confirmOrganisationLink);

        // when
        var confirmationPage = await TestClient.NavigateTo(confirmOrganisationLink);

        // then
        confirmationPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Confirm(organisationId))
            .HasTitle(OrganisationPageTitles.ConfirmYourSelection);
        UserOrganisationsData.SetSecondOrganisationId(organisationId);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldConfirmSecondOrganisationAndNavigateToDashboardPage()
    {
        // given
        var confirmationPage = await GetCurrentPage(OrganisationPagesUrls.Confirm(UserOrganisationsData.SecondaryOrganisationId));
        confirmationPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Confirm(UserOrganisationsData.SecondaryOrganisationId))
            .HasTitle(OrganisationPageTitles.ConfirmYourSelection)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        // then
        var organisationsList = await TestClient.SubmitButton(continueButton, ("IsConfirmed", "True"));

        // then
        organisationsList
            .UrlEndWith(MainPagesUrl.OrganisationList)
            .HasTitle(UserOrganisationsPageTitles.List)
            .HasTotalSummaryCards(2)
            .HasSummaryCardWithTitle(FreshProfileData.OrganisationName)
            .HasSummaryCardWithTitle(UserOrganisationsData.SecondaryOrganisationName)
            .HasLinkWithTestId("add-another-organisation-link", out _)
            .HasLinkWithTestId("manage-profile-link", out _);
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldDisplayErrorMessage_WhenUserTriesToLinkWithTheSameOrganisationAgain()
    {
        // given
        UserOrganisationsData.SetSecondOrganisationId(UserOrganisationsData.SecondaryOrganisationId);
        var confirmationPage = await GetCurrentPage(OrganisationPagesUrls.Confirm(UserOrganisationsData.SecondaryOrganisationId));
        confirmationPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Confirm(UserOrganisationsData.SecondaryOrganisationId))
            .HasTitle(OrganisationPageTitles.ConfirmYourSelection)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        // then
        confirmationPage = await TestClient.SubmitButton(continueButton, ("IsConfirmed", "True"));

        // then
        confirmationPage
            .UrlWithoutQueryEndsWith(OrganisationPagesUrls.Confirm(UserOrganisationsData.SecondaryOrganisationId.ToGuidAsString()))
            .HasTitle(OrganisationPageTitles.ConfirmYourSelection)
            .HasOneValidationMessages($"You are already linked with {UserOrganisationsData.SecondaryOrganisationName}")
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out _);
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldStillBeLinkedToTwoOrganisation_WhenUserIsOnOrganisationsList()
    {
        // given && when
        var organisationsList = await TestClient
            .NavigateTo(MainPagesUrl.OrganisationList);

        // then
        organisationsList
            .UrlEndWith(MainPagesUrl.OrganisationList)
            .HasTitle(UserOrganisationsPageTitles.List)
            .HasTotalSummaryCards(2)
            .HasSummaryCardWithTitle(FreshProfileData.OrganisationName)
            .HasSummaryCardWithTitle(UserOrganisationsData.SecondaryOrganisationName)
            .HasLinkWithTestId("add-another-organisation-link", out _)
            .HasLinkWithTestId("manage-profile-link", out _);
    }
}
