using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.Loans.WWW.Views.Security.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;
using CommonResponse = He.AspNetCore.Mvc.Gds.Components.Constants.CommonResponse;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.SecuritySection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04DirLoansSubIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order04DirLoansSubIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToCheckAnswers_WhenYesIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var dirLoansSubPage = await TestClient.NavigateTo(SecurityPageUrls.DirLoansSub(_applicationId));
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.Yes }, { "DirLoansSubMore", string.Empty } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);

        SetSharedData(SharedKeys.CurrentPageKey, dirLoansSubPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var dirLoansSubPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        dirLoansSubPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", string.Empty } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasOneValidationMessages(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsLongerThan1500Characters()
    {
        // given
        var dirLoansSubPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        dirLoansSubPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasOneValidationMessages(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.SubordinatedLoans));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToCheckAnswers_WhenNoIsSelectedAndAdditionalInformationIs1000Characters()
    {
        // given
        var dirLoansSubPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);
    }
}
