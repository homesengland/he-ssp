using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(210)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order10ProcurementSection : AhpIntegrationTest
{
    public Order10ProcurementSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideProcurements()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteProcurements(SiteData.SiteId),
            SitePageTitles.Procurements,
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            SiteData.Procurements.ToFormInputs(nameof(SiteModel.SiteProcurements)));
    }
}
