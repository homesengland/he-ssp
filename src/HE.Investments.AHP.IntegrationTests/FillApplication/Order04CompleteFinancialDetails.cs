using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.FinancialDetails.Consts;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Extensions;
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
    public async Task Order02_ProvideLandStatus()
    {
        // given
        FinancialDetailsData.GenerateLandStatus();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.LandStatus(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.LandStatusPage,
            FinancialDetailsPagesUrl.LandValueSuffix,
            ("PurchasePrice", FinancialDetailsData.LandStatus.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideLandValue()
    {
        // given
        FinancialDetailsData.GenerateLandValue();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.LandValue(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.LandValuePage,
            FinancialDetailsPagesUrl.OtherApplicationCostsSuffix,
            ("IsOnPublicLand", FinancialDetailsData.IsPublicLand.MapToCommonResponse()),
            ("LandValue", FinancialDetailsData.PublicLandValue.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideOtherApplicationCosts()
    {
        // given
        FinancialDetailsData.GenerateOtherApplicationCosts();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.OtherApplicationCosts(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.OtherApplicationCosts,
            FinancialDetailsPagesUrl.ExpectedContributionsSuffix,
            ("ExpectedWorksCosts", FinancialDetailsData.ExpectedWorksCosts.ToString(CultureInfo.InvariantCulture)),
            ("ExpectedOnCosts", FinancialDetailsData.ExpectedOnCosts.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideExpectedContributions()
    {
        // given
        FinancialDetailsData.GenerateExpectedContributions();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.ExpectedContributions(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.ExpectedContributions,
            FinancialDetailsPagesUrl.GrantsSuffix,
            ("RentalIncomeBorrowing", FinancialDetailsData.ExpectedContributionsRentalIncomeBorrowing.ToString(CultureInfo.InvariantCulture)),
            ("SaleOfHomesOnThisScheme", FinancialDetailsData.ExpectedContributionsSaleOfHomesOnThisScheme.ToString(CultureInfo.InvariantCulture)),
            ("SaleOfHomesOnOtherSchemes", FinancialDetailsData.ExpectedContributionsSaleOfHomesOnOtherSchemes.ToString(CultureInfo.InvariantCulture)),
            ("OwnResources", FinancialDetailsData.ExpectedContributionsOwnResources.ToString(CultureInfo.InvariantCulture)),
            ("RcgfContribution", FinancialDetailsData.ExpectedContributionsRcgfContribution.ToString(CultureInfo.InvariantCulture)),
            ("OtherCapitalSources", FinancialDetailsData.ExpectedContributionsOtherCapitalSources.ToString(CultureInfo.InvariantCulture)),
            ("HomesTransferValue", FinancialDetailsData.ExpectedContributionsHomesTransferValue.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideGrants()
    {
        // given
        FinancialDetailsData.GenerateGrants();

        // when & then
        await TestPage(
            FinancialDetailsPagesUrl.Grants(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.ExpectedGrants,
            FinancialDetailsPagesUrl.CheckAnswersSuffix,
            ("CountyCouncilGrants", FinancialDetailsData.CountyCouncilGrants.ToString(CultureInfo.InvariantCulture)),
            ("DhscExtraCareGrants", FinancialDetailsData.DhscExtraCareGrants.ToString(CultureInfo.InvariantCulture)),
            ("LocalAuthorityGrants", FinancialDetailsData.LocalAuthorityGrants.ToString(CultureInfo.InvariantCulture)),
            ("SocialServicesGrants", FinancialDetailsData.SocialServicesGrants.ToString(CultureInfo.InvariantCulture)),
            ("HealthRelatedGrants", FinancialDetailsData.HealthRelatedGrants.ToString(CultureInfo.InvariantCulture)),
            ("LotteryGrants", FinancialDetailsData.LotteryGrants.ToString(CultureInfo.InvariantCulture)),
            ("OtherPublicBodiesGrants", FinancialDetailsData.OtherPublicBodiesGrants.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_CheckAnswersAndCompleteSection()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(FinancialDetailsPagesUrl.CheckAnswers(ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(FinancialDetailsPagesUrl.CheckAnswersSuffix)
            .HasTitle(FinancialDetailsPageTitles.CheckAnswers)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var schemaInformationSummary = checkAnswersPage.GetSummaryListItems();
        schemaInformationSummary["Purchase price"].Should().Be($"\u00a3{FinancialDetailsData.LandStatus.ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["Current value"].Should().Be($"\u00a3{FinancialDetailsData.PublicLandValue.ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["Public land"].Should().Be("Yes");
        schemaInformationSummary["Works costs"].Should().Be($"\u00a3{FinancialDetailsData.ExpectedWorksCosts.ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["On costs"].Should().Be($"\u00a3{FinancialDetailsData.ExpectedOnCosts.ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["Total scheme costs"]
            .Should()
            .Be(
                $"\u00a3{(FinancialDetailsData.PublicLandValue + FinancialDetailsData.ExpectedWorksCosts + FinancialDetailsData.ExpectedOnCosts).ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["Your contributions"].Should().Be($"\u00a3{FinancialDetailsData.TotalExpectedContributions.ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["Grants from other public bodies"].Should().Be($"\u00a3{FinancialDetailsData.TotalGrants.ToString(CultureInfo.InvariantCulture)}");
        schemaInformationSummary["Total contributions"].Should().Be($"\u00a3{FinancialDetailsData.TotalContributions.ToString(CultureInfo.InvariantCulture)}");

        await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", true.MapToCommonResponse()));

        // then
        SaveCurrentPage();
    }
}
