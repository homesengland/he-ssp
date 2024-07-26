using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW.Views.AllocationClaims.Const;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Order05ManageAllocation.Consts;
using HE.Investments.AHP.IntegrationTests.Order05ManageAllocation.Data.Phase;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order05ManageAllocation;

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
    public async Task Order01_ShouldDisplayClaimsSummaryWithData()
    {
        // given & when
        var summaryPage = await TestClient.NavigateTo(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId));
        var phaseSummary = summaryPage.GetSummaryListItems();

        // then
        summaryPage
            .UrlEndWith(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId))
            .HasTitle(ClaimPageTitles.Summary)
            .HasTitleCaption(AllocationData.AllocationName)
            .HasReturnToAllocationLink()
            .HasSummaryCardWithTitle(PhaseData.PhaseName)
            // .HasElementWithTestIdAndText("grant-details-total-grant-allocated", CurrencyHelper.DisplayPounds(AllocationData.TotalGrantAllocated)) TODO after crm implementation
            .HasElementWithTestIdAndText("grant-details-amount-paid", CurrencyHelper.DisplayPounds(AllocationData.AmountPaid))
            .HasElementWithTestIdAndText("grant-details-amount remaining", CurrencyHelper.DisplayPounds(AllocationData.AmountRemaining));

        phaseSummary[PhaseFields.NumberOfHomes].Value.Should().Be(PhaseData.NumberOfHomes.ToString(CultureInfo.InvariantCulture));
        phaseSummary[PhaseFields.BuildActivityType].Value.Should().Be(PhaseData.BuildActivityType.GetDescription());
        phaseSummary[PhaseFields.AcquisitionForecastClaimDate].Value.Should().Be(PhaseData.AcquisitionForecastClaimDate.ToFormattedDateString());
        phaseSummary[PhaseFields.StartOnSiteForecastClaimDate].Value.Should().Be(PhaseData.StartOnSiteForecastClaimDate.ToFormattedDateString());
        phaseSummary[PhaseFields.CompletionForecastClaimDate].Value.Should().Be(PhaseData.CompletionForecastClaimDate.ToFormattedDateString());

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProceedToPhaseClaimsOverview()
    {
        // given
        var summaryPage =
            await GetCurrentPage(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId));
        var phaseOverviewLink = summaryPage
            .UrlWithoutQueryEndsWith(ClaimsPagesUrl.Summary(UserOrganisationData.OrganisationId, AllocationData.AllocationId))
            .HasTitleCaption(AllocationData.AllocationName)
            .GetLinkByTestId($"view-phase-{ShortGuid.FromString(PhaseData.PhaseId).Value}-link");

        // when
        var overviewPage = await TestClient.NavigateTo(phaseOverviewLink);
        var milestoneSummary = overviewPage.GetSummaryListItems();

        // then
        overviewPage
            .UrlEndWith(ClaimsPagesUrl.Overview(UserOrganisationData.OrganisationId,
                ShortGuid.FromString(AllocationData.AllocationId).Value,
                ShortGuid.FromString(PhaseData.PhaseId).Value))
            .HasTitle(ClaimPageTitles.MilestoneOverview(PhaseData.PhaseName))
            .HasTitleCaption(AllocationData.AllocationName)
            .HasReturnToAllocationLink()
            .HasSummaryCardWithTitle("Acquisition milestone")
            .HasSummaryCardWithTitle("Start on site milestone")
            .HasSummaryCardWithTitle("Practical completion milestone");

        milestoneSummary[MilestoneFields.AmountOfGrantApportioned()].Value.Should().BePoundsOnly(PhaseData.AcquisitionAmountOfGrantApportioned);
        milestoneSummary[MilestoneFields.PercentageOfGrantApportioned()].Value.Should().Be(PhaseData.AcquisitionPercentageOfGrantApportioned);
        milestoneSummary[MilestoneFields.ForecastClaimDate()].Value.Should().Be(PhaseData.AcquisitionForecastClaimDate.ToFormattedDateString());
        milestoneSummary[MilestoneFields.AmountOfGrantApportioned("1")].Value.Should().BePoundsOnly(PhaseData.StartOnSiteAmountOfGrantApportioned);
        milestoneSummary[MilestoneFields.PercentageOfGrantApportioned("2")].Value.Should().Be(PhaseData.StartOnSitePercentageOfGrantApportioned);
        milestoneSummary[MilestoneFields.ForecastClaimDate("3")].Value.Should().Be(PhaseData.StartOnSiteForecastClaimDate.ToFormattedDateString());
        milestoneSummary[MilestoneFields.AmountOfGrantApportioned("4")].Value.Should().BePoundsOnly(PhaseData.CompletionAmountOfGrantApportioned);
        milestoneSummary[MilestoneFields.PercentageOfGrantApportioned("5")].Value.Should().Be(PhaseData.CompletionPercentageOfGrantApportioned);
        milestoneSummary[MilestoneFields.ForecastClaimDate("6")].Value.Should().Be(PhaseData.CompletionForecastClaimDate.ToFormattedDateString());

        SaveCurrentPage();
    }
}
