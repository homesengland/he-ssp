using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.SecuritySection;

[Order(2)]
public class Order02ChargesDebtIntegrationTests : IntegrationTest
{
    public Order02ChargesDebtIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
    [Order(1)]
    public async Task Order01_ShouldMoveToDirectorLoans_WhenContinueButtonIsClicked()
    {
        // given
        var companyPurposePage = await TestClient.NavigateTo(SecurityPageUrls.ChargesDebt(GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey)));
        var continueButton = companyPurposePage.GetGdsSubmitButtonById("continue-button");

        // when
        var directorLoansPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { });

        // then
        directorLoansPage
            .UrlEndWith(SecurityPageUrls.DirectorLoansSuffix)
            .HasTitle("Do you have any director loans against this company?");
    }
}
