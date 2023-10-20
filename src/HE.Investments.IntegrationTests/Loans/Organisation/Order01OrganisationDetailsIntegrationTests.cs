using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework;
using HE.InvestmentLoans.IntegrationTests.IntegrationFramework.Extensions;
using HE.InvestmentLoans.IntegrationTests.Loans.LoansHelpers.Pages;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.InvestmentLoans.IntegrationTests.Loans.Organisation;

[Order(4)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01OrganisationDetailsIntegrationTests : IntegrationTest
{
    public Order01OrganisationDetailsIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenUserOrganisationDetailsPage()
    {
        // given && when
        var userOrganisationPage = await TestClient.NavigateTo(UserOrganisationPagesUrls.UserOrganisationDetails);

        // then
        userOrganisationPage
            .UrlEndWith(UserOrganisationPagesUrls.UserOrganisationDetails)
            .HasTitle($"Manage {UserData.OrganizationName} details")
            .HasElementForTestId("user-organisation-details", out _);
    }
}
