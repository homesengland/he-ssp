using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
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

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02CompleteSchemeInformation : AhpIntegrationTest
{
    public Order02CompleteSchemeInformation(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
        var schemaInformationData = GetSharedDataOrNull<SchemeInformationData>(nameof(SchemeInformationData));
        if (schemaInformationData is null)
        {
            schemaInformationData = new SchemeInformationData();
            SetSharedData(nameof(SchemeInformationData), schemaInformationData);
        }

        SchemeInformationData = schemaInformationData;
    }

    public SchemeInformationData SchemeInformationData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_StartSchemeInformation()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage.HasLinkWithTestId("enter-scheme-information", out var enterSchemeInformationLink);

        // when
        var schemaDetailsPage = await TestClient.NavigateTo(enterSchemeInformationLink);

        // then
        var continueButton = schemaDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.SchemeDetails)
            .HasTitle(SchemeInformationPageTitles.SchemeDetails)
            .GetLinkButton();

        await TestClient.NavigateTo(continueButton);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order2_ProvideFundingDetails()
    {
        // given
        SchemeInformationData.GenerateFundingDetails();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.FundingDetails(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.FundingDetails,
            SchemeInformationPagesUrl.AffordabilitySuffix,
            ("RequiredFunding", SchemeInformationData.RequiredFunding.ToString(CultureInfo.InvariantCulture)),
            ("HousesToDeliver", SchemeInformationData.HousesToDeliver.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order3_ProvideAffordability()
    {
        // given
        SchemeInformationData.GenerateAffordability();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.Affordability(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.Affordability,
            SchemeInformationPagesUrl.SalesRiskSuffix,
            ("AffordabilityEvidence", SchemeInformationData.Affordability));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order4_ProvideSaleRisk()
    {
        // given
        SchemeInformationData.GenerateSalesRisk();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.SalesRisk(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.SalesRisk,
            SchemeInformationPagesUrl.HousingNeedsSuffix,
            ("SalesRisk", SchemeInformationData.SalesRisk));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order5_ProvideHousingNeed()
    {
        // given
        SchemeInformationData.GenerateHousingNeeds();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.HousingNeeds(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.HousingNeeds,
            SchemeInformationPagesUrl.StakeholderDiscussionsSuffix,
            ("MeetingLocalPriorities", SchemeInformationData.HousingNeedsMeetingLocalPriorities),
            ("MeetingLocalHousingNeed", SchemeInformationData.HousingNeedsMeetingLocalHousingNeed));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order6_ProvideStakeholderDiscussions()
    {
        // given
        SchemeInformationData.GenerateStakeholderDiscussions();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.StakeholderDiscussions(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.StakeholderDiscussions,
            SchemeInformationPagesUrl.CheckAnswersSuffix,
            ("StakeholderDiscussionsReport", SchemeInformationData.StakeholderDiscussions));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order7_CheckAnswersAndCompleteSection()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SchemeInformationPagesUrl.CheckAnswers(ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(SchemeInformationPagesUrl.CheckAnswersSuffix)
            .HasTitle(SchemeInformationPageTitles.CheckAnswers)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Application name").WithValue(ApplicationData.ApplicationName);
        summary.Should().ContainKey("Funding required").WhoseValue.Should().BePoundsOnly(SchemeInformationData.RequiredFunding);
        summary.Should().ContainKey("Number of homes").WithValue(SchemeInformationData.HousesToDeliver.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Affordability of Shared Ownership").WithValue(SchemeInformationData.Affordability);
        summary.Should().ContainKey("Sales risk of Shared Ownership").WithValue(SchemeInformationData.SalesRisk);
        summary.Should().ContainKey("Type and tenure of homes").WithValue(SchemeInformationData.HousingNeedsMeetingLocalPriorities);
        summary.Should().ContainKey("Locally identified housing need").WithValue(SchemeInformationData.HousingNeedsMeetingLocalHousingNeed);
        summary.Should().ContainKey("Local stakeholder discussions").WithValue(SchemeInformationData.StakeholderDiscussions);

        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasSectionWithStatus("enter-scheme-information-status", "Completed");
        SaveCurrentPage();
    }
}
