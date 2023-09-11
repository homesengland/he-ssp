using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.TestData;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03MoreInformationAboutOrganizationIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order03MoreInformationAboutOrganizationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        _applicationLoanId = GetSharedData<string>(SharedKeys.ApplicationLoanIdInDraftStatusKey);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenTextIsTooLong()
    {
        // given
        var moreInformationAboutOrganizationPage = await TestClient.NavigateTo(CompanyStructurePagesUrls.MoreInformationAboutOrganization(_applicationLoanId));
        var continueButton = moreInformationAboutOrganizationPage.GetGdsSubmitButtonById("continue-button");

        // when
        moreInformationAboutOrganizationPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "OrganisationMoreInformation", TextTestData.TextWithLenght1501 } });

        // then
        moreInformationAboutOrganizationPage.Url.Should().EndWith(CompanyStructurePagesUrls.MoreInformationAboutOrganizationSuffix);
        moreInformationAboutOrganizationPage.ContainsValidationMessage("Your input cannot be longer than 1000 characters");
        SetSharedData(SharedKeys.CurrentPageKey, moreInformationAboutOrganizationPage);
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageHowManyHomesBuilt_WhenTextHas1000CharsAndContinueButtonIsClicked()
    {
        // given
        var moreInformationAboutOrganizationPage = GetSharedData<IHtmlDocument>(SharedKeys.CurrentPageKey);
        var continueButton = moreInformationAboutOrganizationPage.GetGdsSubmitButtonById("continue-button");

        // when
        var howManyHomesBuiltPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "OrganisationMoreInformation", TextTestData.TextWithLenght1000 } });

        // then
        howManyHomesBuiltPage.Url.Should().EndWith(CompanyStructurePagesUrls.HowManyHomesBuiltSuffix);
        howManyHomesBuiltPage.GetLabel().Should().Be("How many homes in the past three years has your organisation built?");
    }
}
