using System.Diagnostics.CodeAnalysis;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework.Extensions;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.UserOrganisation;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02OrganisationDetailsIntegrationTests : IntegrationTest
{
    public Order02OrganisationDetailsIntegrationTests(IntegrationTestFixture<Program> fixture)
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
