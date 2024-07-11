using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.Common.Utils;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;
using ContractMilestoneStatus = HE.Investments.AHP.Allocation.Contract.Claims.Enum.MilestoneStatus;
using DomainMilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Mappers;

public class MilestoneClaimStatusMapperTests : TestBase<MilestoneClaimStatusMapper>
{
    private readonly DateTime _today = new(2024, 07, 20, 0, 0, 0, DateTimeKind.Local);

    public MilestoneClaimStatusMapperTests()
    {
        var dateTimeProvider = CreateAndRegisterDependencyMock<IDateTimeProvider>();

        dateTimeProvider.Setup(x => x.Now).Returns(_today);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Submitted, ContractMilestoneStatus.Submitted)]
    [InlineData(DomainMilestoneStatus.UnderReview, ContractMilestoneStatus.UnderReview)]
    [InlineData(DomainMilestoneStatus.Approved, ContractMilestoneStatus.Approved)]
    [InlineData(DomainMilestoneStatus.Rejected, ContractMilestoneStatus.Rejected)]
    [InlineData(DomainMilestoneStatus.Reclaimed, ContractMilestoneStatus.Reclaimed)]
    public void ShouldReturnTheSameStatus_WhenStatusCanBeMappedDirectly(DomainMilestoneStatus domainStatus, ContractMilestoneStatus contractStatus)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, _today);

        // then
        result.Should().Be(contractStatus);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Undefined, 15)]
    [InlineData(DomainMilestoneStatus.Draft, 15)]
    [InlineData(DomainMilestoneStatus.Undefined, 150)]
    [InlineData(DomainMilestoneStatus.Draft, 150)]
    public void ShouldReturnUndefinedStatus_WhenItIsMoreThan14DaysBeforeForecastClaimDateAndStatusIs(DomainMilestoneStatus domainStatus, int days)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, _today.AddDays(days));

        // then
        result.Should().Be(ContractMilestoneStatus.Undefined);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Undefined, 14)]
    [InlineData(DomainMilestoneStatus.Draft, 14)]
    [InlineData(DomainMilestoneStatus.Undefined, 7)]
    [InlineData(DomainMilestoneStatus.Draft, 7)]
    public void ShouldReturnDueSoonStatus_WhenItIsBetween14And7DaysBeforeForecastClaimDateAndStatusIs(DomainMilestoneStatus domainStatus, int days)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, _today.AddDays(days));

        // then
        result.Should().Be(ContractMilestoneStatus.DueSoon);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Undefined, 6)]
    [InlineData(DomainMilestoneStatus.Draft, 6)]
    [InlineData(DomainMilestoneStatus.Undefined, -6)]
    [InlineData(DomainMilestoneStatus.Draft, -6)]
    public void ShouldReturnDueStatus_WhenItIsBetween6DaysBeforeAnd6DaysAfterForecastClaimDateAndStatusIs(DomainMilestoneStatus domainStatus, int days)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, _today.AddDays(days));

        // then
        result.Should().Be(ContractMilestoneStatus.Due);
    }

    [Theory]
    [InlineData(DomainMilestoneStatus.Undefined, 7)]
    [InlineData(DomainMilestoneStatus.Draft, 7)]
    [InlineData(DomainMilestoneStatus.Undefined, 150)]
    [InlineData(DomainMilestoneStatus.Draft, 150)]
    public void ShouldReturnOverdueStatus_WhenItIsMoreThan6DaysAfterForecastClaimDateAndStatusIs(DomainMilestoneStatus domainStatus, int days)
    {
        // given & when
        var result = TestCandidate.MapStatus(domainStatus, _today.AddDays(-days));

        // then
        result.Should().Be(ContractMilestoneStatus.Overdue);
    }
}
