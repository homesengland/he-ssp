using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using HE.Investments.TestsUtils.Helpers;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[Order(504)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04ClaimStartOnSiteMilestone : ClaimMilestoneTestBase
{
    public Order04ClaimStartOnSiteMilestone(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    protected override ClaimData ClaimData => new(
        MilestoneType.StartOnSite,
        new DateDetails("01", "03", "2025"),
        Confirmation: true);

    protected override DateTime ClaimSubmissionDate => new(2025, 03, 03, 12, 00, 00, DateTimeKind.Local);

    protected override MilestoneStatus? StatusOnSubmission => MilestoneStatus.DueSoon;

    protected override void AssertClaimSummary(IDictionary<string, SummaryItem> summary)
    {
        summary.Should().NotContainKey("Cost incurred");
        summary.Should()
            .ContainKey("Amount of grant apportioned to start on site milestone")
            .WhoseValue.Value.Should()
            .BePoundsOnly(PhaseData.StartOnSiteAmountOfGrantApportioned);
        summary.Should().ContainKey("Start on site achievement date").WithValue("1 March 2025");
    }
}
