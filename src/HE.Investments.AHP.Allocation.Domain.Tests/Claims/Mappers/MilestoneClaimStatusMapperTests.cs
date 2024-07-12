using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using ContractMilestoneStatus = HE.Investments.AHP.Allocation.Contract.Claims.Enum.MilestoneStatus;
using DomainMilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Mappers;

public class MilestoneClaimStatusMapperTests : TestBase<MilestoneClaimStatusMapper>
{
    [Theory]
    [InlineData(DomainMilestoneStatus.Submitted, ContractMilestoneStatus.Submitted)]
    [InlineData(DomainMilestoneStatus.UnderReview, ContractMilestoneStatus.UnderReview)]
    [InlineData(DomainMilestoneStatus.Approved, ContractMilestoneStatus.Approved)]
    [InlineData(DomainMilestoneStatus.Rejected, ContractMilestoneStatus.Rejected)]
    [InlineData(DomainMilestoneStatus.Reclaimed, ContractMilestoneStatus.Reclaimed)]
    public void ShouldReturnTheSameStatus_WhenStatusCanBeMappedDirectly(DomainMilestoneStatus domainStatus, ContractMilestoneStatus expectedStatus)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, MilestoneDueStatus.Due);

        // then
        result.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Undefined, MilestoneDueStatus.Undefined, ContractMilestoneStatus.Undefined)]
    [InlineData(DomainMilestoneStatus.Undefined, MilestoneDueStatus.Due, ContractMilestoneStatus.Due)]
    [InlineData(DomainMilestoneStatus.Undefined, MilestoneDueStatus.DueSoon, ContractMilestoneStatus.DueSoon)]
    [InlineData(DomainMilestoneStatus.Undefined, MilestoneDueStatus.Overdue, ContractMilestoneStatus.Overdue)]
    [InlineData(DomainMilestoneStatus.Draft, MilestoneDueStatus.Undefined, ContractMilestoneStatus.Undefined)]
    [InlineData(DomainMilestoneStatus.Draft, MilestoneDueStatus.Due, ContractMilestoneStatus.Due)]
    [InlineData(DomainMilestoneStatus.Draft, MilestoneDueStatus.DueSoon, ContractMilestoneStatus.DueSoon)]
    [InlineData(DomainMilestoneStatus.Draft, MilestoneDueStatus.Overdue, ContractMilestoneStatus.Overdue)]
    public void ShouldReturnDueStatus_WhenStatusIs(DomainMilestoneStatus domainStatus, MilestoneDueStatus dueStatus, ContractMilestoneStatus expectedStatus)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, dueStatus);

        // then
        result.Should().Be(expectedStatus);
    }
}
