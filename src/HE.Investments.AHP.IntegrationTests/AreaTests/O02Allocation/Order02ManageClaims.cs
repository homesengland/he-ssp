using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW.Views.AllocationClaims.Const;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Consts;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data.Phase;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Extensions;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Helpers;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[Order(502)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02ManageClaims : AhpIntegrationTest
{
    public Order02ManageClaims(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        PhaseData = ReturnSharedData<PhaseData>();
    }

    public PhaseData PhaseData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayClaimsSummary()
    {
        // given
        var summaryPage = await GetCurrentPage(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId));
        summaryPage
            .UrlEndWith(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId))
            .HasTitle(ClaimPageTitles.Summary)
            .HasTitleCaption(AllocationData.AllocationName)
            .HasReturnToAllocationLink()
            .HasGrantDetails(AllocationData.TotalGrantAllocated, AllocationData.AmountPaid, AllocationData.AmountRemaining)
            .HasSummaryCardWithTitle(PhaseData.PhaseName, out var phaseSummaryCard);
        phaseSummaryCard.HasLinkWithText("View phase", out _);

        // when
        var phaseSummary = phaseSummaryCard.GetSummaryListItems();

        // then
        phaseSummary.Should().ContainKey(PhaseFields.NumberOfHomes).WithValue(PhaseData.NumberOfHomes.ToString(CultureInfo.InvariantCulture));
        phaseSummary.Should().ContainKey(PhaseFields.BuildActivityType).WithValue(PhaseData.BuildActivityType);
        phaseSummary.Should().ContainKey(PhaseFields.AcquisitionForecastClaimDate).WithValue(PhaseData.AcquisitionForecastClaimDate);
        phaseSummary.Should().ContainKey(PhaseFields.StartOnSiteForecastClaimDate).WithValue(PhaseData.StartOnSiteForecastClaimDate);
        phaseSummary.Should().ContainKey(PhaseFields.CompletionForecastClaimDate).WithValue(PhaseData.CompletionForecastClaimDate);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProceedToPhaseClaimsOverview()
    {
        // given
        var summaryPage = await GetCurrentPage(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId));
        summaryPage
            .UrlEndWith(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId))
            .HasTitle(ClaimPageTitles.Summary)
            .HasSummaryCardWithTitle(PhaseData.PhaseName, out var summaryCard);
        summaryCard.HasLinkWithText("View phase", out var phaseOverviewLink);

        var phaseId = phaseOverviewLink.ExtractParameter("phaseId", ClaimsPagesUrl.Overview(string.Empty, string.Empty, "{phaseId}"));
        PhaseData.SetPhaseId(phaseId);

        // when
        var overviewPage = await TestClient.NavigateTo(phaseOverviewLink);

        // then
        overviewPage
            .UrlEndWith(ClaimsPagesUrl.Overview(
                UserOrganisationData.OrganisationId,
                ShortGuid.FromString(AllocationData.AllocationId).Value,
                ShortGuid.FromString(PhaseData.PhaseId).Value))
            .HasTitle(ClaimPageTitles.MilestoneOverview(PhaseData.PhaseName))
            .HasTitleCaption(AllocationData.AllocationName)
            .HasReturnToAllocationLink()
            .HasSummaryCardWithTitle("Acquisition milestone", out var acquisitionSummaryCard)
            .HasSummaryCardWithTitle("Start on site milestone", out var startOnSiteSummaryCard)
            .HasSummaryCardWithTitle("Practical completion milestone", out var completionSummaryCard);

        var acquisitionSummary = acquisitionSummaryCard.GetSummaryListItems();
        acquisitionSummary.Should()
            .ContainKey(MilestoneFields.AmountOfGrantApportioned)
            .WhoseValue.Value.Should()
            .BePoundsOnly(PhaseData.AcquisitionAmountOfGrantApportioned);
        acquisitionSummary.Should().ContainKey(MilestoneFields.PercentageOfGrantApportioned).WithValue(PhaseData.AcquisitionPercentageOfGrantApportioned);
        acquisitionSummary.Should().ContainKey(MilestoneFields.ForecastClaimDate).WithValue(PhaseData.AcquisitionForecastClaimDate);

        var startOnSiteSummary = startOnSiteSummaryCard.GetSummaryListItems();
        startOnSiteSummary.Should()
            .ContainKey(MilestoneFields.AmountOfGrantApportioned)
            .WhoseValue.Value.Should()
            .BePoundsOnly(PhaseData.StartOnSiteAmountOfGrantApportioned);
        startOnSiteSummary.Should().ContainKey(MilestoneFields.PercentageOfGrantApportioned).WithValue(PhaseData.StartOnSitePercentageOfGrantApportioned);
        startOnSiteSummary.Should().ContainKey(MilestoneFields.ForecastClaimDate).WithValue(PhaseData.StartOnSiteForecastClaimDate);

        var completionSummary = completionSummaryCard.GetSummaryListItems();
        completionSummary.Should()
            .ContainKey(MilestoneFields.AmountOfGrantApportioned)
            .WhoseValue.Value.Should()
            .BePoundsOnly(PhaseData.CompletionAmountOfGrantApportioned);
        completionSummary.Should().ContainKey(MilestoneFields.PercentageOfGrantApportioned).WithValue(PhaseData.CompletionPercentageOfGrantApportioned);
        completionSummary.Should().ContainKey(MilestoneFields.ForecastClaimDate).WithValue(PhaseData.CompletionForecastClaimDate);

        SaveCurrentPage();
    }
}
