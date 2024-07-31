using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Views.AllocationClaims.Const;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data.Phase;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Extensions;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.TestsUtils.Extensions;
using HE.Investments.TestsUtils.Helpers;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public abstract class ClaimMilestoneTestBase : AhpIntegrationTest
{
    protected ClaimMilestoneTestBase(
        AhpIntegrationTestFixture fixture,
        ITestOutputHelper output)
        : base(fixture, output)
    {
        PhaseData = ReturnSharedData<PhaseData>();
    }

    protected abstract ClaimData ClaimData { get; }

    protected abstract DateTime ClaimSubmissionDate { get; }

    protected PhaseData PhaseData { get; }

    protected ClaimPagesUrl ClaimPagesUrl => new(UserOrganisationData.OrganisationId, AllocationData.AllocationId, PhaseData.PhaseId, ClaimData.MilestoneType);

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldClickClaimMilestoneLink()
    {
        // given
        var overviewPage = await GetCurrentPage(ClaimsPagesUrl.Overview(UserOrganisationData.OrganisationId, AllocationData.AllocationId, PhaseData.PhaseId));
        overviewPage.HasLinkWithTestId(ClaimMilestoneLinkTestId(), out var claimMilestoneLink);

        // when
        await TestClient.NavigateTo(claimMilestoneLink);

        // then
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvideCostsIncurred()
    {
        if (ClaimData.CostsIncurred.HasValue)
        {
            await TestClaimQuestionPage(
                ClaimPagesUrl.CostsIncurred,
                ClaimPageTitles.CostsIncurred(PhaseData.PhaseName, ClaimData.MilestoneType),
                ClaimPagesUrl.AchievementDate,
                ("CostsIncurred", ClaimData.CostsIncurred!.Value.ToString()));
        }
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideAchievementDate()
    {
        // given
        DateTimeManipulator.TravelInTimeTo(ClaimSubmissionDate);

        // when && then
        await TestClaimQuestionPage(
            ClaimPagesUrl.AchievementDate,
            ClaimPageTitles.AchievementDate(PhaseData.PhaseName, ClaimData.MilestoneType),
            ClaimPagesUrl.Confirmation,
            ("AchievementDate.Day", ClaimData.AchievementDate.Day!),
            ("AchievementDate.Month", ClaimData.AchievementDate.Month!),
            ("AchievementDate.Year", ClaimData.AchievementDate.Year!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideConfirmation()
    {
        await TestClaimQuestionPage(
            ClaimPagesUrl.Confirmation,
            ClaimPageTitles.Confirmation,
            ClaimPagesUrl.CheckAnswers,
            ("IsConfirmed", ClaimData.Confirmation ? "checked" : string.Empty));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldDisplayCorrectSummary()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ClaimPagesUrl.CheckAnswers);
        checkAnswersPage
            .UrlEndWith(ClaimPagesUrl.CheckAnswers)
            .HasTitle(ClaimPageTitles.CheckAnswers)
            .HasSubmitButton(out _, "Submit claim");

        // when
        var summary = checkAnswersPage.GetSummaryListItems();

        // then
        AssertClaimSummary(summary);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldSubmitClaim()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ClaimPagesUrl.CheckAnswers);
        checkAnswersPage
            .UrlEndWith(ClaimPagesUrl.CheckAnswers)
            .HasTitle(ClaimPageTitles.CheckAnswers)
            .HasSubmitButton(out var submitClaimButton, "Submit claim");

        // when
        var overviewPage = await TestClient.SubmitButton(submitClaimButton);

        // then
        overviewPage.HasNoElementWithTestId(ClaimMilestoneLinkTestId())
            .HasMilestoneStatusTag(ClaimData.MilestoneType, MilestoneStatus.Submitted);
        SaveCurrentPage();
    }

    protected abstract void AssertClaimSummary(IDictionary<string, SummaryItem> summary);

    private string ClaimMilestoneLinkTestId() => $"claim-{ClaimData.MilestoneType.ToString().ToIdTag()}-milestone";

    private Task<IHtmlDocument> TestClaimQuestionPage(
        string startPageUrl,
        string expectedPageTitle,
        string expectedPageUrlAfterContinue,
        params (string InputName, string Value)[] inputs)
    {
        Action<IHtmlDocument>[] additionalChecksForExpectedPage =
        [
            page => page.HasCancelAndReturnToAllocationLink(),
        ];

        return TestQuestionPage(
            startPageUrl,
            expectedPageTitle,
            expectedPageUrlAfterContinue,
            additionalChecksForExpectedPage,
            inputs);
    }
}
