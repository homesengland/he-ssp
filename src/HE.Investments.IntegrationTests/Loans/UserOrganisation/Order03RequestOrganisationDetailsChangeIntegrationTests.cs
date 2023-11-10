using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Services.Notifications;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.UserOrganisation;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03RequestOrganisationDetailsChangeIntegrationTests : IntegrationTest
{
    private const string TestSkipReason =
        "User can only request a change of organisation details once, then this page will not be visible. You should add a request to delete your organization details";

    public Order03RequestOrganisationDetailsChangeIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = TestSkipReason)]
    [Order(1)]
    public async Task Order01_ShouldOpenRequestDetailsChangePage_WhenLinkIsClickedOnTheOrganisationDetailsPage()
    {
        // given
        var organisationDetails = await TestClient.NavigateTo(UserOrganisationPagesUrls.UserOrganisationDetails);

        // when
        var linkToOrganisationDetailsChange = organisationDetails.GetAnchorElementById("request-details-change-link");
        var detailsChangePage = await TestClient.NavigateTo(linkToOrganisationDetailsChange);

        // then
        detailsChangePage
            .UrlEndWith(UserOrganisationPagesUrls.RequestDetailsChange)
            .HasTitle("Request to change organisation details")
            .HasGdsSubmitButton("save-and-continue-button", out _);

        SetSharedData(SharedKeys.CurrentPageKey, detailsChangePage);
    }

    [Fact(Skip = TestSkipReason)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenInputsAreMissing()
    {
        // given
        var detailsChangePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = detailsChangePage.GetGdsSubmitButtonById("save-and-continue-button");

        // when
        detailsChangePage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string>
            {
                { "Name", string.Empty },
                { "PhoneNumber", string.Empty },
                { "AddressLine1", string.Empty },
                { "TownOrCity", string.Empty },
                { "Postcode", string.Empty },
            });

        // then
        detailsChangePage
            .UrlEndWith(UserOrganisationPagesUrls.RequestDetailsChange)
            .HasTitle("Request to change organisation details")
            .ContainsValidationMessages(
                OrganisationErrorMessages.MissingRegisteredOrganisationName,
                OrganisationErrorMessages.MissingPhoneNumber,
                OrganisationErrorMessages.MissingOrganisationAddress,
                OrganisationErrorMessages.MissingOrganisationTownOrCity,
                OrganisationErrorMessages.MissingOrganisationPostCode);
    }

    [Fact(Skip = TestSkipReason)]
    [Order(3)]
    public async Task Order03_ShouldDisplayValidationError_WhenInputsAreTooLong()
    {
        // given
        var detailsChangePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = detailsChangePage.GetGdsSubmitButtonById("save-and-continue-button");

        // when
        detailsChangePage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string>
            {
                { "Name", TextTestData.TextThatExceedsShortInputLimit },
                { "PhoneNumber", TextTestData.TextThatExceedsShortInputLimit },
                { "AddressLine1", TextTestData.TextThatExceedsShortInputLimit },
                { "AddressLine2", TextTestData.TextThatExceedsShortInputLimit },
                { "TownOrCity", TextTestData.TextThatExceedsShortInputLimit },
                { "County", TextTestData.TextThatExceedsShortInputLimit },
                { "Postcode", "random postcode" },
            });

        // then
        detailsChangePage
            .UrlEndWith(UserOrganisationPagesUrls.RequestDetailsChange)
            .HasTitle("Request to change organisation details")
            .ContainsValidationMessages(
                ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.RequestToChangeOrganisationDetails),
                OrganisationErrorMessages.InvalidOrganisationPostcode);
    }

    [Fact(Skip = TestSkipReason)]
    [Order(4)]
    public async Task Order04_ShouldRedirectToOrganisationDetailsPage_WhenInputsAreCorrect()
    {
        // given
        var detailsChangePage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = detailsChangePage.GetGdsSubmitButtonById("save-and-continue-button");

        // when
        detailsChangePage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string>
            {
                { "Name", "my organisation" },
                { "PhoneNumber", "123 456 789" },
                { "AddressLine1", "Main Street" },
                { "AddressLine2", "100" },
                { "TownOrCity", "London" },
                { "County", "My county" },
                { "Postcode", "CH65 1AY" },
            });

        // then
        detailsChangePage
            .UrlEndWith(UserOrganisationPagesUrls.UserOrganisationDetails)
            .HasTitle($"Manage {UserData.OrganizationName} details")
            .HasSuccessNotificationBanner("Your request to change your organisation details has been sent for review.")
            .HasInsetText(OrganisationDetailsView.PendingRequestByYou);
    }
}
