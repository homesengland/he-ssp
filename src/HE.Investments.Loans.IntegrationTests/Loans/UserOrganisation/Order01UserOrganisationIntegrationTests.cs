using System.Diagnostics.CodeAnalysis;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.IntegrationTests.IntegrationFramework;
using HE.Investments.Loans.IntegrationTests.Loans.LoansHelpers.Pages;
using HE.Investments.Loans.WWW;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.Loans.IntegrationTests.Loans.UserOrganisation;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01UserOrganisationIntegrationTests : IntegrationTest
{
    public Order01UserOrganisationIntegrationTests(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = LoansConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenUserOrganisationPage()
    {
        // given && when
        var userOrganisationPage = await TestClient.NavigateTo(UserOrganisationPagesUrls.UserOrganisation);

        // then
        userOrganisationPage
            .UrlEndWith(UserOrganisationPagesUrls.UserOrganisation)
            .HasLinkButtonForTestId("user-organisation-start-new-application-levelling-up-home-building-fund", out _)
            .HasElementForTestId("user-organisation-limited-user", out _);
    }
}
