using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Security.Consts;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;

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
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextWithLenght1501 } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasOneValidationMessages("Request to change reason why cannot be subordinated must be 1500 characters or less");
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
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextWithLenght1000 } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle(SecurityPageTitles.CheckAnswers);
    }
}
