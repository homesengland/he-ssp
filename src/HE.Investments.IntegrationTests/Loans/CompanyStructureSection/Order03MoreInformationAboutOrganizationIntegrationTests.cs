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
    private const string CurrentPageKey = nameof(CurrentPageKey);

    public Order03MoreInformationAboutOrganizationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenTextIsTooLong()
    {
        // given
        var moreInformationAboutOrganizationPage = GetSharedData<IHtmlDocument>(CurrentPageKey);
        var continueButton = moreInformationAboutOrganizationPage.GetGdsSubmitButtonById("continue-button");

        // when
        moreInformationAboutOrganizationPage = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "OrganisationMoreInformation", TextTestData.TextWithLenght1501 } });

        // then
        moreInformationAboutOrganizationPage.Url.Should().EndWith(CompanyStructurePagesUrls.MoreInformationAboutOrganizationSuffix);
        moreInformationAboutOrganizationPage.ContainsValidationMessage("Your input cannot be longer than 1000 characters");
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldMoveToNextPageHowManyHomesBuilt_WhenTextHas1000CharsAndContinueButtonIsClicked()
    {
        // given
        var moreInformationAboutOrganizationPage = GetSharedData<IHtmlDocument>(CurrentPageKey);
        var continueButton = moreInformationAboutOrganizationPage.GetGdsSubmitButtonById("continue-button");

        // when
        var howManyHomesBuilt = await TestClient.SubmitButton(
            continueButton, new Dictionary<string, string> { { "OrganisationMoreInformation", TextTestData.TextWithLenght1000 } });

        // then
        howManyHomesBuilt.Url.Should().EndWith(CompanyStructurePagesUrls.HowManyHomesBuiltSuffix);
        howManyHomesBuilt.GetLabel().Should().Be("How many homes in the past three years has your organisation built?");
    }
}
