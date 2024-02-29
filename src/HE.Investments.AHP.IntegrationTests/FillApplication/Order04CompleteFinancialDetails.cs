using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.FinancialDetails.Consts;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Assertions;
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
        taskListPage.HasLinkWithTestId("enter-financial-details", out var enterSchemeInformationLink);

        // when
        var schemaDetailsPage = await TestClient.NavigateTo(enterSchemeInformationLink);

        // then
        var continueButton = schemaDetailsPage
            .UrlEndWith(FinancialDetailsPagesUrl.Start)
            .HasTitle(FinancialDetailsPageTitles.StartPage)
            .GetLinkButton();

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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
        await TestQuestionPage(
            FinancialDetailsPagesUrl.OtherApplicationCosts(ApplicationData.ApplicationId),
            FinancialDetailsPageTitles.OtherSchemeCosts,
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
        await TestQuestionPage(
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
        await TestQuestionPage(
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
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Purchase price").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.LandStatus);
        summary.Should().ContainKey("Current value").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.PublicLandValue);
        summary.Should().ContainKey("Public land").WithValue(YesNoType.Yes);
        summary.Should().ContainKey("Works costs").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.ExpectedWorksCosts);
        summary.Should().ContainKey("On costs").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.ExpectedOnCosts);
        summary.Should().ContainKey("Total scheme costs").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.PublicLandValue + FinancialDetailsData.ExpectedWorksCosts + FinancialDetailsData.ExpectedOnCosts);
        summary.Should().ContainKey("Your contributions").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.TotalExpectedContributions);
        summary.Should().ContainKey("Grants from other public bodies").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.TotalGrants);
        summary.Should().ContainKey("Total contributions").WhoseValue.Value.Should().BePoundsOnly(FinancialDetailsData.TotalContributions);

        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasSectionWithStatus("enter-financial-details-status", "Completed");
        SaveCurrentPage();
    }
}
