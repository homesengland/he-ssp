using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using HE.Investments.TestsUtils.Helpers;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation;

[Order(503)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03ClaimAcquisitionMilestone : ClaimMilestoneTestBase
{
    public Order03ClaimAcquisitionMilestone(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    protected override ClaimData ClaimData => new(
        MilestoneType.Acquisition,
        new DateDetails("10", "05", "2024"),
        Confirmation: true,
        CostsIncurred: true);

    protected override DateTime ClaimSubmissionDate => new(2024, 05, 11, 12, 00, 00, DateTimeKind.Local);

    protected override void AssertClaimSummary(IDictionary<string, SummaryItem> summary)
    {
        summary.Should().ContainKey("Cost incurred").WithValue(ClaimData.CostsIncurred!.Value.MapToCommonResponse());
        summary.Should()
            .ContainKey("Amount of grant apportioned to acquisition milestone")
            .WhoseValue.Value.Should()
            .BePoundsOnly(PhaseData.AcquisitionAmountOfGrantApportioned);
        summary.Should().ContainKey("Acquisition achievement date").WithValue("10 May 2024");
    }
}
