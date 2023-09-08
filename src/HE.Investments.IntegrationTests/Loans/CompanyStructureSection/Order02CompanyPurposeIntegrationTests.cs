using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02CompanyPurposeIntegrationTests : IntegrationTest
{
    private const string CurrentPageKey = nameof(CurrentPageKey);

    public Order02CompanyPurposeIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToNextPageMoreInformationAboutOrganization_WhenContinueButtonIsClicked()
    {
        // given
        var companyPurposePage = GetSharedData<IHtmlDocument>(CurrentPageKey);
        var continueButton = companyPurposePage.GetGdsSubmitButtonById("continue-button");

        // when
        var moreInformationAboutOrganizationPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "Purpose", CommonResponse.Yes } });

        // then
        moreInformationAboutOrganizationPage.Url.Should().EndWith(CompanyStructurePagesUrls.MoreInformationAboutOrganizationSuffix);
        moreInformationAboutOrganizationPage.GetLabel().Should().Be("Provide more information about your organisation");
        SetSharedData(CurrentPageKey, moreInformationAboutOrganizationPage);
    }
}
