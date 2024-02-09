using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.ProjectSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order11LocalAuthorityTests : IntegrationTest
{
    private readonly string _projectId;

    private readonly string _applicationLoanId;

    public Order11LocalAuthorityTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
        _projectId = UserData.ProjectInDraftStateId;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenPhraseIsNotProvided()
    {
        // given
        var localAuthoritySearchPage = await TestClient.NavigateTo(ProjectPagesUrls.LocalAuthoritySearch(_applicationLoanId, _projectId));

        var continueButton = localAuthoritySearchPage.GetGdsSubmitButtonById("search-local-authority-button");

        // when
        localAuthoritySearchPage = await TestClient.SubmitButton(continueButton, new Dictionary<string, string> { { "Phrase", string.Empty } });

        // then
        localAuthoritySearchPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthoritySearchSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthority)
            .ContainsValidationMessage(ValidationErrorMessage.LocalAuthorityNameIsEmpty);

        SetSharedData(SharedKeys.CurrentPageKey, localAuthoritySearchPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldReturnPageLocalAuthorityNotFound_WhenPhraseIsARandomText()
    {
        // given
        var localAuthoritySearchPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = localAuthoritySearchPage.GetGdsSubmitButtonById("search-local-authority-button");

        // when
        var localAuthorityNotFoundPage = await TestClient.SubmitButton(continueButton, new Dictionary<string, string> { { "Phrase", "very random text" } });

        // then
        localAuthorityNotFoundPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthorityNotFoundSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthorityNoMatch);

        SetSharedData(SharedKeys.CurrentPageKey, localAuthorityNotFoundPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldAssignEmptyLocalAuthority_WhenChoosingToAddLocalAuthorityLater()
    {
        // given
        var localAuthorityNotFoundPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var assignEmptyLocalAuthorityLink = localAuthorityNotFoundPage.GetAnchorElementById("assign-empty-local-authority-link");

        // when
        var ownershipPage = await TestClient.NavigateTo(assignEmptyLocalAuthorityLink);

        // then
        ownershipPage
            .UrlEndWith(ProjectPagesUrls.OwnershipSuffix)
            .HasTitle(ProjectPageTitles.Ownership);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldReturnToLocalAuthoritySearchPage_WhenChoosingStartAgainLink()
    {
        // given
        var localAuthorityNotFoundPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var localAuthoritySearchLink = localAuthorityNotFoundPage.GetAnchorElementById("local-authority-search-link");

        // when
        var localAuthoritySearchPage = await TestClient.NavigateTo(localAuthoritySearchLink);

        // then
        localAuthoritySearchPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthoritySearchSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthority);

        SetSharedData(SharedKeys.CurrentPageKey, localAuthoritySearchPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldReturnLocalAuthoritiesForSearchedPhrase_WhenCorrectPhraseIsProvided()
    {
        // given
        var localAuthoritySearchPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = localAuthoritySearchPage.GetGdsSubmitButtonById("search-local-authority-button");

        // when
        var localAuthorityResultPage = await TestClient.SubmitButton(continueButton, new Dictionary<string, string> { { "Phrase", UserData.LocalAuthorityName } });

        // then
        localAuthorityResultPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrls.LocalAuthoritySearchResultSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthorityResult);

        SetSharedData(SharedKeys.CurrentPageKey, localAuthorityResultPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldReturnLocalAuthorityConfirmPage_WhenLocalAuthorityIsSelected()
    {
        // given
        var localAuthorityResultPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        // when
        localAuthorityResultPage.HasAnchorElementById($"local-authority-link-{UserData.LocalAuthorityId}", out var liverpoolAuthorityLink);
        var localAuthorityConfirmPage = await TestClient.NavigateTo(liverpoolAuthorityLink);

        // then
        localAuthorityConfirmPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrls.LocalAuthorityConfirmSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthorityConfirm);

        SetSharedData(SharedKeys.CurrentPageKey, localAuthorityConfirmPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldReturnToLocalAuthoritySearchPage_WhenChoosingNotToConfirmLocalAuthority()
    {
        // given
        var localAuthorityConfirmPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = localAuthorityConfirmPage.GetGdsSubmitButtonById("continue-button");

        // when
        var localAuthoritySearchPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Response", CommonResponse.No } });

        // then
        localAuthoritySearchPage
            .UrlEndWith(ProjectPagesUrls.LocalAuthoritySearchSuffix)
            .HasTitle(ProjectPageTitles.LocalAuthority);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldReturnOwnershipPage_WhenLocalAuthorityIsConfirmed()
    {
        // given
        var localAuthorityConfirmPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = localAuthorityConfirmPage.GetGdsSubmitButtonById("continue-button");

        // when
        var ownershipPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Response", CommonResponse.Yes } });

        // then
        ownershipPage
            .UrlEndWith(ProjectPagesUrls.OwnershipSuffix)
            .HasTitle(ProjectPageTitles.Ownership);
    }
}
