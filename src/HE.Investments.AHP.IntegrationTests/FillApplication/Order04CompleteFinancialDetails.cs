using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.FinancialDetails.Consts;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillApplication;

[Order(4)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04CompleteFinancialDetails : AhpIntegrationTest
{
    public Order04CompleteFinancialDetails(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        var financialDetailsData = GetSharedDataOrNull<FinancialDetailsData>(nameof(FinancialDetailsData));
        if (financialDetailsData is null)
        {
            financialDetailsData = new FinancialDetailsData();
            SetSharedData(nameof(FinancialDetailsData), financialDetailsData);
        }

        FinancialDetailsData = financialDetailsData;
    }

    public FinancialDetailsData FinancialDetailsData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_StartFinancialDetails()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage.HasLinkWithId("enter-financial-details", out var enterSchemeInformationLink);

        // when
        var schemaDetailsPage = await TestClient.NavigateTo(enterSchemeInformationLink);

        // then
        schemaDetailsPage
            .UrlEndWith(FinancialDetailsPagesUrl.Start)
            .HasTitle(FinancialDetailsPageTitles.StartPage)
            .HasGdsLinkButton("continue-button", out var continueButton);

        await TestClient.NavigateTo(continueButton);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order2_ProvideLandStatus()
    {
        // given
        FinancialDetailsData.GenerateLandStatus();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.LandStatus(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.LandStatusPage,
            FinancialDetailsPagesUrl.LandValueSuffix,
            ("PurchasePrice", FinancialDetailsData.LandStatus.ToString()!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order3_ProvideLandValue()
    {
        // given
        FinancialDetailsData.GenerateLandValue();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.LandValue(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.LandValuePage,
            FinancialDetailsPagesUrl.OtherApplicationCostsSuffix,
            ("IsOnPublicLand", FinancialDetailsData.IsPublicLand.ToString().ToLowerInvariant()),
            ("LandValue", FinancialDetailsData.PublicLandValue.ToString()!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order3_ProvideOtherApplicationCosts()
    {
        // given
        FinancialDetailsData.GenerateOtherApplicationCosts();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.OtherApplicationCosts(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.OtherApplicationCosts,
            FinancialDetailsPagesUrl.ExpectedContributionsSuffix,
            ("ExpectedWorksCosts", FinancialDetailsData.ExpectedWorksCosts.ToString()!),
            ("ExpectedOnCosts", FinancialDetailsData.ExpectedOnCosts.ToString()!));
    }
}
