using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order10LocationIntegrationTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order10LocationIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldRedirectToLocalAuthoritySearch_WhenNoLocationTypeWasSelected()
    {
        // given
        var locationPage = await TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        var localAuthoritySearchPage = await TestClient.SubmitButton(continueButton, (nameof(ProjectViewModel.LocationOption), string.Empty));

        // then
        localAuthoritySearchPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthoritySearchSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthority);

        SetSharedData(SharedKeys.CurrentPageKey, locationPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldShowValidationMessage_WhenCoordinatesWasSelectedAndNoValueWasProvided()
    {
        // given
        var locationPage = await GetCurrentPage(() =>
            TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId)));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        locationPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.LocationOption), ProjectFormOption.Coordinates),
            (nameof(ProjectViewModel.LocationCoordinates), null!));

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location)
            .HasOneValidationMessages(ValidationErrorMessage.EnterCoordinates);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldShowValidationMessage_WhenCoordinatesExceedsLongInputLength()
    {
        // given
        var locationPage = await GetCurrentPage(() =>
            TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId)));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        locationPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.LocationOption), ProjectFormOption.Coordinates),
            (nameof(ProjectViewModel.LocationCoordinates), TextTestData.TextThatExceedsLongInputLimit));

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location)
            .HasOneValidationMessages(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationCoordinates));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldShowValidationMessage_WhenTitleNumberWasSelectedAndNoValueWasProvided()
    {
        // given
        var locationPage = await GetCurrentPage(() =>
            TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId)));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        locationPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.LocationOption), ProjectFormOption.LandRegistryTitleNumber),
            (nameof(ProjectViewModel.LocationLandRegistry), string.Empty));

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location)
            .HasOneValidationMessages(ValidationErrorMessage.EnterLandRegistryTitleNumber);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldShowValidationMessage_WhenTitleNumberExceedsLongInputLength()
    {
        // given
        var locationPage = await GetCurrentPage(() =>
            TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId)));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        locationPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.LocationOption), ProjectFormOption.LandRegistryTitleNumber),
            (nameof(ProjectViewModel.LocationLandRegistry), TextTestData.TextThatExceedsLongInputLimit));

        // then
        locationPage
            .UrlEndWith(ProjectPagesUrls.LocationSuffix)
            .HasTitle(ProjectPageTitles.Location)
            .HasOneValidationMessages(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationLandRegistry));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_RedirectTolocalAuthoritySearchPage_WhenCoordinatesWasSelected()
    {
        // given
        var locationPage = await GetCurrentPage(() =>
            TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId)));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        var localAuthoritySearchPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.LocationOption), ProjectFormOption.Coordinates),
            (nameof(ProjectViewModel.LocationCoordinates), TextTestData.TextThatNotExceedsLongInputLimit));

        // then
        localAuthoritySearchPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthoritySearchSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthority);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShowCoordinatesAtCheckAnswersPage()
    {
        // given
        var checkAnswersPage = await TestClient.NavigateTo(ProjectPagesUrls.CheckAnswers(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId));

        // when
        var projectSummary = checkAnswersPage.GetSummaryListItems();

        // then
        projectSummary[ProjectFieldNames.Coordinates].Value.Should().Be(TextTestData.TextThatNotExceedsLongInputLimit);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_RedirectToLocalAuthoritySearchPage_WhenTitleNumberWasSelected()
    {
        // given
        var locationPage = await GetCurrentPage(() =>
            TestClient.NavigateTo(ProjectPagesUrls.Location(UserOrganisationData.OrganisationId, _applicationLoanId, _projectId)));
        var continueButton = locationPage.GetGdsSubmitButtonById("continue-button");

        // when
        var localAuthoritySearchPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectViewModel.LocationOption), ProjectFormOption.LandRegistryTitleNumber),
            (nameof(ProjectViewModel.LocationLandRegistry), TextTestData.TextThatNotExceedsLongInputLimit));

        // then
        localAuthoritySearchPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthoritySearchSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthority);
    }
}
