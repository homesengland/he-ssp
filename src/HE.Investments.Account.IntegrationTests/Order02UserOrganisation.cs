using System.Diagnostics.CodeAnalysis;
using System.Web;
using FluentAssertions;
using HE.Investments.Account.IntegrationTests.Extensions;
using HE.Investments.Account.IntegrationTests.Framework;
using HE.Investments.Account.IntegrationTests.Pages;
using HE.Investments.Account.WWW;
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
            .HasActionLink(MainPagesUrl.UserOrganisationDetails, out _)
            .HasActionLink(MainPagesUrl.ManageUsers, out _);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldNavigateToOrganisationDetails()
    {
        // given
        var dashboardPage = await GetCurrentPage(MainPagesUrl.Dashboard);
        dashboardPage
            .UrlEndWith(MainPagesUrl.Dashboard)
            .HasActionLink(MainPagesUrl.UserOrganisationDetails, out var organisationDetailsLink);

        // when
        var organisationDetailsPage = await TestClient.NavigateTo(organisationDetailsLink);

        // then
        var organisationDetails = organisationDetailsPage
            .UrlEndWith(MainPagesUrl.UserOrganisationDetails)
            .GetSummaryListItems();

        organisationDetails["Registered company name"].Should().NotBeNull().And.NotBe("Not provided");
        organisationDetails["Organisation phone number"].Should().NotBeNull();
        organisationDetails["Organisation address"].Should().NotBeNull();
        organisationDetails["Companies House Registration number"].Should().NotBeNull();

        UserOrganisationData.SetOrganisationName(organisationDetails["Registered company name"]);
        SaveCurrentPage();
    }

    [SkippableFact(typeof(SkipException), Skip = AccountConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldNavigateToRequestToChangeOrganisationDetails()
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
    [Order(4)]
    public async Task Order04_ShouldDisplayValidationError_WhenInputsAreInvalid()
    {
        // given
        var tooLongShortString = new string(Enumerable.Repeat('a', 120).ToArray());
        var currentPage = await GetCurrentPage(MainPagesUrl.ChangeOrganisationDetails);
        currentPage
            .UrlWithoutQueryEndsWith(MainPagesUrl.ChangeOrganisationDetails)
            .HasTitle(UserOrganisationPageTitles.ChangeOrganisationDetails)
            .HasGdsBackLink()
            .HasGdsSubmitButton(out var sendRequestButton, "Send request");

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
            "Enter your organisation's town or city",
            "Enter a full UK postcode");
    }
}
