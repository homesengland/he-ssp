using System.Diagnostics.CodeAnalysis;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.Application.Order02Sections.CompanyStructureSection;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02CompanyPurposeIntegrationTests : IntegrationTest
{
    public Order02CompanyPurposeIntegrationTests(LoansIntegrationTestFixture fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldMoveToNextPageMoreInformationAboutOrganization_WhenContinueButtonIsClicked()
    {
        // given
        var companyPurposePage =
            await TestClient.NavigateTo(CompanyStructurePagesUrls.CompanyPurpose(UserOrganisationData.OrganisationId, UserData.LoanApplicationIdInDraftState));
        var continueButton = companyPurposePage.GetGdsSubmitButtonById("continue-button");

        // when
        var moreInformationAboutOrganizationPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { "Purpose", CommonResponse.Yes } });

        // then
        moreInformationAboutOrganizationPage
            .UrlEndWith(CompanyStructurePagesUrls.MoreInformationAboutOrganizationSuffix)
            .HasTitle("Organisation structure");
    }
}
