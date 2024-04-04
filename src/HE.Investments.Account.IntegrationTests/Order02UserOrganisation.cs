using System.Diagnostics.CodeAnalysis;
using System.Web;
using FluentAssertions;
using HE.Investments.Account.IntegrationTests.Data;
using HE.Investments.Account.IntegrationTests.Extensions;
using HE.Investments.Account.IntegrationTests.Framework;
using HE.Investments.Account.IntegrationTests.Pages;
using HE.Investments.Account.WWW;
using HE.Investments.Account.WWW.Views.User;
using HE.Investments.Account.WWW.Views.UserOrganisation;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Account.IntegrationTests;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02UserOrganisation : AccountIntegrationTest
{
    public Order02UserOrganisation(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenOrganisationDashboard()
    {
        // given & when
        var dashboardPage = await TestClient
            .NavigateTo(MainPagesUrl.Dashboard);

        // then
        dashboardPage
            .UrlEndWith(MainPagesUrl.Dashboard)
            .HasMatchingTitle(".+'s Homes England account$")
            .HasActionLink(MainPagesUrl.UserOrganisationDetails, out _)
            .HasActionLink(MainPagesUrl.ManageUsers, out _)
            .HasActionLink(MainPagesUrl.ProfileDetails, out _);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldOpenProfileDetails()
    {
        // given
        var currentPage = await GetCurrentPage(MainPagesUrl.Dashboard);
        currentPage
            .UrlEndWith(MainPagesUrl.Dashboard)
            .HasActionLink(MainPagesUrl.ProfileDetails, out var profileDetailsLink);

        // when
        var nextPage = await TestClient.NavigateTo(profileDetailsLink);

        // then
        nextPage.UrlWithoutQueryEndsWith(MainPagesUrl.ProfileDetails)
            .HasTitle(UserPageTitles.ProfileDetails);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldChangeTelephoneNumber()
    {
        // given
        var currentPage = await GetCurrentPage(MainPagesUrl.ProfileDetails);
        currentPage.UrlWithoutQueryEndsWith(MainPagesUrl.ProfileDetails)
            .HasTitle(UserPageTitles.ProfileDetails)
            .HasSaveAndContinueButton(out var submitButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            submitButton,
            ("TelephoneNumber", TelephoneNumberGenerator.GenerateWithCountryCode()),
            ("SecondaryTelephoneNumber", TelephoneNumberGenerator.GenerateWithoutCountryCode()));

        // then
        nextPage.UrlEndWith(MainPagesUrl.Dashboard);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldOpenManageUsers()
    {
        // given
        var currentPage = await GetCurrentPage(MainPagesUrl.Dashboard);
        currentPage
            .UrlEndWith(MainPagesUrl.Dashboard)
            .HasActionLink(MainPagesUrl.ManageUsers, out var manageUsersLink);

        // when
        var nextPage = await TestClient.NavigateTo(manageUsersLink);

        // then
        nextPage
            .UrlEndWith(MainPagesUrl.ManageUsers)
            .HasMatchingTitle("^Manage users at .+")
            .HasBackLink(out _)
            .HasManageUserRow(LoginData.Email, out _);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldOpenManageUser()
    {
        // given
        var currentPage = await GetCurrentPage(MainPagesUrl.ManageUsers);
        currentPage
            .UrlEndWith(MainPagesUrl.ManageUsers)
            .HasManageUserRow(LoginData.Email, out var manageMyUserRow);
        var manageMyUserLink = manageMyUserRow.GetManageUserLink();

        // when
        var nextPage = await TestClient.NavigateTo(manageMyUserLink);

        // then
        nextPage
            .UrlEndWith(MainPagesUrl.ManageUser(LoginData.UserGlobalId))
            .HasMatchingTitle("^Manage .+'s role")
            .HasBackLink(out _)
            .HasRadio("Role", value: "Admin")
            .HasSaveAndContinueButton();
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldNavigateBackwardsToDashboard()
    {
        // given
        var currentPage = await GetCurrentPage(MainPagesUrl.ManageUser(LoginData.UserGlobalId));
        currentPage.HasBackLink(out var backLink);

        // when
        var nextPage = await TestClient.NavigateTo(backLink);

        // then
        nextPage
            .UrlEndWith(MainPagesUrl.ManageUsers)
            .HasBackLink(out backLink);

        // when
        nextPage = await TestClient.NavigateTo(backLink);

        // then
        nextPage
            .UrlEndWith(MainPagesUrl.Dashboard)
;
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldNavigateToOrganisationDetails()
    {
        // given
        var dashboardPage = await GetCurrentPage(MainPagesUrl.Dashboard);
        dashboardPage
            .UrlEndWith(MainPagesUrl.Dashboard)
            .HasActionLink(MainPagesUrl.UserOrganisationDetails, out var organisationDetailsLink);

        // when
        var organisationDetailsPage = await TestClient.NavigateTo(organisationDetailsLink);

        // then
        var summary = organisationDetailsPage
            .UrlEndWith(MainPagesUrl.UserOrganisationDetails)
            .GetSummaryListItems();

        summary.Should().ContainKey("Registered company name").WhoseValue.Value.Should().NotBeNull().And.NotBe("Not provided");
        summary.Should().ContainKey("Organisation phone number").WhoseValue.Value.Should().NotBeNull();
        summary.Should().ContainKey("Organisation address").WhoseValue.Value.Should().NotBeNull();
        summary.Should().ContainKey("Companies House Registration number").WhoseValue.Value.Should().NotBeNull();

        UserOrganisationData.SetOrganisationName(summary["Registered company name"].Value);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldNavigateToRequestToChangeOrganisationDetails()
    {
        // given
        var organisationDetailsPage = await GetCurrentPage(MainPagesUrl.UserOrganisationDetails);
        organisationDetailsPage
            .UrlEndWith(MainPagesUrl.UserOrganisationDetails)
            .HasTitle(UserOrganisationPageTitles.Details(UserOrganisationData.OrganisationName))
            .HasLinkWithTestId("request-details-change-link", out var changeOrganisationDetailsLink);

        // when
        var changeOrganisationDetailsPage = await TestClient.NavigateTo(changeOrganisationDetailsLink);

        // then
        changeOrganisationDetailsPage
            .UrlEndWith(MainPagesUrl.ChangeOrganisationDetails)
            .HasTitle(UserOrganisationPageTitles.ChangeOrganisationDetails)
            .HasInput("Name", value: HttpUtility.HtmlDecode(UserOrganisationData.OrganisationName));
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ShouldDisplayValidationError_WhenInputsAreInvalid()
    {
        // given
        var tooLongShortString = new string(Enumerable.Repeat('a', 120).ToArray());
        var currentPage = await GetCurrentPage(MainPagesUrl.ChangeOrganisationDetails);
        currentPage
            .UrlWithoutQueryEndsWith(MainPagesUrl.ChangeOrganisationDetails)
            .HasTitle(UserOrganisationPageTitles.ChangeOrganisationDetails)
            .HasBackLink(out _)
            .HasSubmitButton(out var sendRequestButton, "Send request");

        // when
        var changeOrganisationDetailsPage = await TestClient.SubmitButton(
            sendRequestButton,
            ("Name", UserOrganisationData.OrganisationName),
            ("PhoneNumber", tooLongShortString),
            ("AddressLine1", tooLongShortString),
            ("AddressLine2", string.Empty),
            ("TownOrCity", string.Empty),
            ("County", string.Empty),
            ("Postcode", "invalid postcode"));

        // then
        changeOrganisationDetailsPage.ContainsValidationMessages(
            "The Address Line 1 must be 100 characters or less",
            "Enter your organisation town or city",
            "Enter a full UK postcode");
    }
}
