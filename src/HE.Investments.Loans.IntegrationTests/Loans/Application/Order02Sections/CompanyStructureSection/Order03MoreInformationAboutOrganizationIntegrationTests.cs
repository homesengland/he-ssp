using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03MoreInformationAboutOrganizationIntegrationTests : IntegrationTest
{
    private readonly string _applicationLoanId;

    public Order03MoreInformationAboutOrganizationIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
        _applicationLoanId = UserData.LoanApplicationIdInDraftState;
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayValidationError_WhenTextIsTooLong()
    {
        // given
        var moreInformationAboutOrganizationPage =
            await TestClient.NavigateTo(CompanyStructurePagesUrls.MoreInformationAboutOrganization(UserOrganisationData.OrganisationId, _applicationLoanId));
        var continueButton = moreInformationAboutOrganizationPage.GetGdsSubmitButtonById("continue-button");

        // when
        moreInformationAboutOrganizationPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "OrganisationMoreInformation", TextTestData.TextThatExceedsLongInputLimit } });

        // then
        moreInformationAboutOrganizationPage
            .UrlEndWith(CompanyStructurePagesUrls.MoreInformationAboutOrganizationSuffix)
            .HasOneValidationMessages("The organisation more information must be 1500 characters or less");

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
            continueButton,
            new Dictionary<string, string> { { "OrganisationMoreInformation", TextTestData.TextThatNotExceedsLongInputLimit } });

        // then
        howManyHomesBuiltPage
            .UrlEndWith(CompanyStructurePagesUrls.HowManyHomesBuiltSuffix)
            .HasLabelTitle("How many homes in the past three years has your organisation built?");
    }
}
