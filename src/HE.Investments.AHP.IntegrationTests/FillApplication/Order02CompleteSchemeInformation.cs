using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
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
        var schemaInformationData = GetSharedDataOrNull<SchemaInformationData>(nameof(SchemaInformationData));
        if (schemaInformationData is null)
        {
            schemaInformationData = new SchemaInformationData();
            SetSharedData(nameof(SchemaInformationData), schemaInformationData);
        }

        SchemaInformationData = schemaInformationData;
    }

    public SchemaInformationData SchemaInformationData { get; }

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
            .UrlEndWith(SchemaInformationPagesUrl.SchemaDetails)
            .HasTitle(SchemaInformationPageTitles.SchemeDetails)
            .HasGdsLinkButton("continue-button", out var continueButton);

        await TestClient.NavigateTo(continueButton);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order2_ProvideFundingDetails()
    {
        // given
        var fundingDetailsPage = await GetCurrentPage(SchemaInformationPagesUrl.FundingDetails(ApplicationData.ApplicationId));
        fundingDetailsPage
            .UrlEndWith(SchemaInformationPagesUrl.FundingDetailsSuffix)
            .HasTitle(SchemaInformationPageTitles.FundingDetails)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        SchemaInformationData.GenerateFundingDetails();
        var affordabilityPage = await TestClient.SubmitButton(
            continueButton,
            ("RequiredFunding", SchemaInformationData.RequiredFunding.ToString()!),
            ("HousesToDeliver", SchemaInformationData.HousesToDeliver.ToString()!));

        // then
        affordabilityPage.UrlEndWith(SchemaInformationPagesUrl.AffordabilitySuffix);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order3_ProvideAffordability()
    {
        // given
        var affordabilityPage = await GetCurrentPage(SchemaInformationPagesUrl.Affordability(ApplicationData.ApplicationId));
        affordabilityPage
            .UrlEndWith(SchemaInformationPagesUrl.AffordabilitySuffix)
            .HasTitle(SchemaInformationPageTitles.Affordability)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        SchemaInformationData.GenerateAffordability();
        var salesRiskPage = await TestClient.SubmitButton(
            continueButton,
            ("AffordabilityEvidence", SchemaInformationData.Affordability));

        // then
        salesRiskPage.UrlEndWith(SchemaInformationPagesUrl.SalesRiskSuffix);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order4_ProvideSaleRisk()
    {
        // given
        var salesRiskPage = await GetCurrentPage(SchemaInformationPagesUrl.SalesRisk(ApplicationData.ApplicationId));
        salesRiskPage
            .UrlEndWith(SchemaInformationPagesUrl.SalesRiskSuffix)
            .HasTitle(SchemaInformationPageTitles.SalesRisk)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        SchemaInformationData.GenerateSalesRisk();
        var housingNeedsPage = await TestClient.SubmitButton(
            continueButton,
            ("SalesRisk", SchemaInformationData.SalesRisk));

        // then
        housingNeedsPage.UrlEndWith(SchemaInformationPagesUrl.HousingNeedsSuffix);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order5_ProvideHousingNeed()
    {
        // given
        var housingNeedsPage = await GetCurrentPage(SchemaInformationPagesUrl.HousingNeeds(ApplicationData.ApplicationId));
        housingNeedsPage
            .UrlEndWith(SchemaInformationPagesUrl.HousingNeedsSuffix)
            .HasTitle(SchemaInformationPageTitles.HousingNeeds)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        SchemaInformationData.GenerateHousingNeeds();
        var stakeholderDiscussionsPage = await TestClient.SubmitButton(
            continueButton,
            ("MeetingLocalPriorities", SchemaInformationData.HousingNeedsMeetingLocalPriorities),
            ("MeetingLocalHousingNeed", SchemaInformationData.HousingNeedsMeetingLocalHousingNeed));

        // then
        stakeholderDiscussionsPage.UrlEndWith(SchemaInformationPagesUrl.StakeholderDiscussionsSuffix);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order6_ProvideStakeholderDiscussions()
    {
        // given
        var stakeholderDiscussionsPage = await GetCurrentPage(SchemaInformationPagesUrl.StakeholderDiscussions(ApplicationData.ApplicationId));
        stakeholderDiscussionsPage
            .UrlEndWith(SchemaInformationPagesUrl.StakeholderDiscussionsSuffix)
            .HasTitle(SchemaInformationPageTitles.StakeholderDiscussions)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        SchemaInformationData.GenerateStakeholderDiscussions();
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton,
            ("StakeholderDiscussionsReport", SchemaInformationData.StakeholderDiscussions));

        // then
        checkAnswersPage.UrlEndWith(SchemaInformationPagesUrl.CheckAnswersSuffix);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order7_CheckAnswersAndCompleteSection()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SchemaInformationPagesUrl.CheckAnswers(ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(SchemaInformationPagesUrl.CheckAnswersSuffix)
            .HasTitle(SchemaInformationPageTitles.CheckAnswers)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var schemaInformationSummary = checkAnswersPage.GetSummaryListItems();
        schemaInformationSummary["Application name"].Should().Be(ApplicationData.ApplicationName);
        schemaInformationSummary["Funding required"].Should().Be(SchemaInformationData.RequiredFunding.ToString());
        schemaInformationSummary["Number of homes"].Should().Be(SchemaInformationData.HousesToDeliver.ToString());
        schemaInformationSummary["Affordability of Shared Ownership"].Should().Be(SchemaInformationData.Affordability);
        schemaInformationSummary["Sales risk of Shared Ownership"].Should().Be(SchemaInformationData.SalesRisk);
        schemaInformationSummary["Type and tenure of homes"].Should().Be(SchemaInformationData.HousingNeedsMeetingLocalPriorities);
        schemaInformationSummary["Locally identified housing need"].Should().Be(SchemaInformationData.HousingNeedsMeetingLocalHousingNeed);
        schemaInformationSummary["Local stakeholder discussions"].Should().Be(SchemaInformationData.StakeholderDiscussions);

        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", true.ToString().ToLowerInvariant()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskListSuffix);
        SaveCurrentPage();
    }
}
