using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AngleSharp.Html.Dom;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.InvestmentLoans.WWW.Views.Security.Consts;
using Microsoft.AspNetCore.Components.Web;
using Xunit;
using Xunit.Extensions.Ordering;
using FormOptions = HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02ChargesDebtIntegrationTests : IntegrationTest
{
    public Order02ChargesDebtIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenYesIsSelectedAndAdditionalInformationIsNotProvided()
    {
        // given
        var chargesDebtPage = await TestClient.NavigateTo(SecurityPageUrls.ChargesDebt(GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey)));
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        chargesDebtPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", string.Empty } });

        // then
        chargesDebtPage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt)
            .ContainsValidationMessage(ValidationErrorMessage.EnterMoreDetails);

        SetSharedData(SharedKeys.CurrentPageKey, chargesDebtPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldDisplayValidationError_WhenNoIsSelectedAndAdditionalInformationIsLongerThan1500Characters()
    {
        // given
        var chargesDebtPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);

        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextWithLenght1501 } });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.ChargesDebtSuffix)
            .HasTitle(SecurityPageTitles.ChargesDebt)
            .ContainsValidationMessage(ValidationErrorMessage.LongInputLengthExceeded(FormOptions.FieldNameForInputLengthValidation.ReasonWhyCannotBeSubordinated));
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldMoveToDirectorLoans_WhenNoIsSelected()
    {
        // given
        var chargesDebtPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.No }, { "ChargesDebtCompanyInfo", string.Empty } });

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
        var chargesDebtPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = chargesDebtPage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "ChargesDebtCompany", CommonResponse.Yes }, { "ChargesDebtCompanyInfo", TextTestData.TextWithLenght1000 } });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle(SecurityPageTitles.DirLoans);
    }
}
