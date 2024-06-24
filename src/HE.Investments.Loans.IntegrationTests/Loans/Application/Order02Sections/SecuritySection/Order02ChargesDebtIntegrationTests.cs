using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW.Views.Security.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;
using CommonResponse = He.AspNetCore.Mvc.Gds.Components.Constants.CommonResponse;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.SecuritySection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02ChargesDebtIntegrationTests : IntegrationTest
{
    public Order02ChargesDebtIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var chargesDebtPage =
            await TestClient.NavigateTo(SecurityPageUrls.ChargesDebt(UserOrganisationData.OrganisationId, UserData.LoanApplicationIdInDraftState));
        var continueButton = chargesDebtPage.GetContinueButton();

        // when
        chargesDebtPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", string.Empty } });

        // then
        chargesDebtPage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt)
            .ContainsOnlyOneValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetSharedData(SharedKeys.CurrentPageKey, chargesDebtPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsLongerThan1500Characters()
    {
        // given
        var continueButton = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey).GetContinueButton();

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string>
            {
                { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextThatExceedsLongInputLimit },
            });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt)
            .ContainsOnlyOneValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.Debenture));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToDirectorLoans_WhenNoIsSelected()
    {
        // given
        var continueButton = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey).GetContinueButton();

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.No }, { "ChargesDebtCompanyInfo", string.Empty } });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle(SecurityPageTitles.DirLoans);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldMoveToDirectorLoans_WhenYesIsSelectedAndAdditionalDataProvided()
    {
        // given
        var continueButton = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey).GetContinueButton();

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string>
            {
                { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextThatNotExceedsLongInputLimit },
            });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle(SecurityPageTitles.DirLoans);
    }
}
