using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
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
        taskListPage.HasLinkWithId("enter-scheme-information", out var enterSchemeInformationLink);

        // when
        var schemaDetailsPage = await TestClient.NavigateTo(enterSchemeInformationLink);

        // then
        schemaDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.SchemeDetails)
            .HasTitle(SchemeInformationPageTitles.SchemeDetails)
            .HasGdsLinkButton("continue-button", out var continueButton);

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
        await TestPage(
            SchemeInformationPagesUrl.FundingDetails(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.FundingDetails,
            SchemeInformationPagesUrl.AffordabilitySuffix,
            ("RequiredFunding", SchemeInformationData.RequiredFunding.ToString()!),
            ("HousesToDeliver", SchemeInformationData.HousesToDeliver.ToString()!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order3_ProvideAffordability()
    {
        // given
        SchemeInformationData.GenerateAffordability();

        // when & then
        await TestPage(
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
        await TestPage(
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
        await TestPage(
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
        await TestPage(
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
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var schemaInformationSummary = checkAnswersPage.GetSummaryListItems();
        schemaInformationSummary["Application name"].Should().Be(ApplicationData.ApplicationName);
        schemaInformationSummary["Funding required"].Should().Be(SchemeInformationData.RequiredFunding.ToString());
        schemaInformationSummary["Number of homes"].Should().Be(SchemeInformationData.HousesToDeliver.ToString());
        schemaInformationSummary["Affordability of Shared Ownership"].Should().Be(SchemeInformationData.Affordability);
        schemaInformationSummary["Sales risk of Shared Ownership"].Should().Be(SchemeInformationData.SalesRisk);
        schemaInformationSummary["Type and tenure of homes"].Should().Be(SchemeInformationData.HousingNeedsMeetingLocalPriorities);
        schemaInformationSummary["Locally identified housing need"].Should().Be(SchemeInformationData.HousingNeedsMeetingLocalHousingNeed);
        schemaInformationSummary["Local stakeholder discussions"].Should().Be(SchemeInformationData.StakeholderDiscussions);

        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", true.ToString().ToLowerInvariant()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskListSuffix);
        SaveCurrentPage();
    }
}
