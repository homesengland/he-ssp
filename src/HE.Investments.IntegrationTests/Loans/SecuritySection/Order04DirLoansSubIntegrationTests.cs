using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;

[Order(2)]
public class Order04DirLoansSubIntegrationTests : IntegrationTest
{
    private readonly string _applicationId;

    public Order04DirLoansSubIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToCheckAnswers_WhenYesIsSelectedAndContinueButtonIsClicked()
    {
        // given
        var companyPurposePage = await TestClient.NavigateTo(SecurityPageUrls.DirLoansSub(_applicationId));
        var continueButton = companyPurposePage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.Yes }, { "DirLoansSubMore", string.Empty } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle("Check your answers");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var dirLoansSubPage = await TestClient.NavigateTo(SecurityPageUrls.DirLoansSub(_applicationId));
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        dirLoansSubPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", string.Empty } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasValidationMessages(ValidationErrorMessage.EnterMoreDetails);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsLongerThan1000Characters()
    {
        // given
        var dirLoansSubPage = await TestClient.NavigateTo(SecurityPageUrls.DirLoansSub(_applicationId));
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        dirLoansSubPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextWithLenght1501 } });

        // then
        dirLoansSubPage
            .UrlEndWith(SecurityPageUrls.DirLoansSubSuffix)
            .HasValidationMessages("Your input cannot be longer than 1000 characters");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToCheckAnswers_WhenNoIsSelectedAndAdditionalInformationIs1000Characters()
    {
        // given
        var dirLoansSubPage = await TestClient.NavigateTo(SecurityPageUrls.DirLoansSub(_applicationId));
        var continueButton = dirLoansSubPage.GetGdsSubmitButtonById("continue-button");

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "DirLoansSub", CommonResponse.No }, { "DirLoansSubMore", TextTestData.TextWithLenght1000 } });

        // then
        checkAnswersPage
            .UrlEndWith(SecurityPageUrls.CheckYourAnswersSuffix)
            .HasTitle("Check your answers");
    }
}
