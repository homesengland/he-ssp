using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Helpers;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[Order(505)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05ClaimCompletionMilestone : ClaimMilestoneTestBase
{
    public Order05ClaimCompletionMilestone(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    protected override ClaimData ClaimData => new(
        MilestoneType.Completion,
        new DateDetails("10", "01", "2026"),
        Confirmation: true);

    protected override DateTime ClaimSubmissionDate => new(2026, 01, 11, 12, 00, 00, DateTimeKind.Local);

    protected override void AssertClaimSummary(IDictionary<string, SummaryItem> summary)
    {
        summary.Should().NotContainKey("Cost incurred");
        summary.Should().ContainKey("Amount of grant apportioned to practical completion milestone"); // TODO: AB#104059 Assert milestone tranche amount
        summary.Should().ContainKey("Practical completion achievement date").WithValue("10 January 2026");
    }
}
