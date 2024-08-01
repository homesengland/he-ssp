using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Strategies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.PhaseCrmMapperTests;

public class MapToDomainTests : TestBase<PhaseCrmMapper>
{
    [Fact]
    public void ShouldMapPhase_WhenOnlyCompletionMilestoneIsProvided()
    {
        // given
        var allocation = new AllocationBasicInfoBuilder().Build();
        var forecastClaimDate = new DateTime(2024, 07, 10, 0, 0, 0, DateTimeKind.Utc);
        var completionClaim = new MilestoneClaimDto
        {
            Type = (int)invln_Milestone.PC,
            AmountOfGrantApportioned = 1000000,
            PercentageOfGrantApportioned = 1,
            Status = (int)invln_ClaimExternalStatus.Draft,
            ForecastClaimDate = forecastClaimDate,
        };
        var dto = new PhaseClaimsDto
        {
            AllocationId = allocation.Id.Value,
            Id = "phase-id",
            Name = "My first phase",
            NumberOfHomes = 10,
            NewBuildActivityType = (int)invln_NewBuildActivityType.WorksOnly,
            RehabBuildActivityType = null,
            AcquisitionMilestone = null,
            StartOnSiteMilestone = null,
            CompletionMilestone = completionClaim,
        };
        var milestoneAvailabilityStrategy = CreateMilestoneAvailabilityStrategyMock(true);

        // when
        var result = TestCandidate.MapToDomain(dto, allocation, false, milestoneAvailabilityStrategy);

        // then
        result.Should().NotBeNull();
        result.Allocation.Should().Be(allocation);
        result.Id.Value.Should().Be("phase-id");
        result.Name.Value.Should().Be("My first phase");
        result.NumberOfHomes.Value.Should().Be(10);
        result.BuildActivityType.Should().Be(BuildActivityType.WorksOnly);
        result.AcquisitionMilestone.Should().BeNull();
        result.StartOnSiteMilestone.Should().BeNull();
        result.CompletionMilestone.Should().NotBeNull();
        result.CompletionMilestone.Should().BeOfType<DraftMilestoneClaim>();
        result.CompletionMilestone.Type.Should().Be(MilestoneType.Completion);
        result.CompletionMilestone.Status.Should().Be(MilestoneStatus.Draft);
        result.CompletionMilestone.GrantApportioned.Should().Be(new GrantApportioned(1000000, 0.01m));
        result.CompletionMilestone.ClaimDate.Should().Be(new ClaimDate(forecastClaimDate));
        result.IsOnlyCompletionMilestone.Should().BeTrue();
    }

    [Fact]
    public void ShouldMapPhase_WhenAllMilestonesAreProvided()
    {
        // given
        var allocation = new AllocationBasicInfoBuilder().Build();
        var forecastClaimDate = new DateTime(2024, 07, 10, 0, 0, 0, DateTimeKind.Utc);
        var dto = new PhaseClaimsDto
        {
            AllocationId = allocation.Id.Value,
            Id = "phase-id",
            Name = "My first phase",
            NumberOfHomes = 10,
            NewBuildActivityType = (int)invln_NewBuildActivityType.WorksOnly,
            RehabBuildActivityType = null,
            AcquisitionMilestone = MilestoneFactory((int)invln_Milestone.Acquisition),
            StartOnSiteMilestone = MilestoneFactory((int)invln_Milestone.SoS),
            CompletionMilestone = MilestoneFactory((int)invln_Milestone.PC),
        };
        var milestoneAvailabilityStrategy = CreateMilestoneAvailabilityStrategyMock();

        // when
        var result = TestCandidate.MapToDomain(dto, allocation, false, milestoneAvailabilityStrategy);

        // then
        result.Should().NotBeNull();
        result.Allocation.Should().Be(allocation);
        result.Id.Value.Should().Be("phase-id");
        result.Name.Value.Should().Be("My first phase");
        result.NumberOfHomes.Value.Should().Be(10);
        result.BuildActivityType.Should().Be(BuildActivityType.WorksOnly);
        AssertClaim(MilestoneType.Acquisition, result.AcquisitionMilestone);
        AssertClaim(MilestoneType.StartOnSite, result.StartOnSiteMilestone);
        AssertClaim(MilestoneType.Completion, result.CompletionMilestone);
        result.IsOnlyCompletionMilestone.Should().BeFalse();

        MilestoneClaimDto MilestoneFactory(int type) =>
            new()
            {
                Type = type,
                AmountOfGrantApportioned = 1000000,
                PercentageOfGrantApportioned = 1,
                Status = (int)invln_ClaimExternalStatus.Approved,
                ForecastClaimDate = forecastClaimDate,
            };

        void AssertClaim(MilestoneType milestoneType, MilestoneClaimBase? claim)
        {
            claim.Should().NotBeNull().And.BeOfType<SubmittedMilestoneClaim>();
            claim!.Type.Should().Be(milestoneType);
            claim.Status.Should().Be(MilestoneStatus.Approved);
            claim.GrantApportioned.Should().Be(new GrantApportioned(1000000, 0.01m));
            claim.ClaimDate.Should().Be(new ClaimDate(forecastClaimDate));
        }
    }

    [Fact]
    public void ShouldMapMilestoneWithoutClaim_WhenStatusIsZero()
    {
        // given
        var allocation = new AllocationBasicInfoBuilder().Build();
        var forecastClaimDate = new DateTime(2024, 07, 10, 0, 0, 0, DateTimeKind.Utc);
        var completionClaim = new MilestoneClaimDto
        {
            Type = (int)invln_Milestone.PC,
            Status = 0,
            AmountOfGrantApportioned = 1000000,
            PercentageOfGrantApportioned = 1,
            ForecastClaimDate = forecastClaimDate,
        };
        var dto = new PhaseClaimsDto
        {
            AllocationId = allocation.Id.Value,
            Id = "phase-id",
            Name = "My first phase",
            NumberOfHomes = 10,
            NewBuildActivityType = (int)invln_NewBuildActivityType.WorksOnly,
            CompletionMilestone = completionClaim,
        };
        var milestoneAvailabilityStrategy = CreateMilestoneAvailabilityStrategyMock(true);

        // when
        var result = TestCandidate.MapToDomain(dto, allocation, false, milestoneAvailabilityStrategy);

        // then
        result.CompletionMilestone.Should().BeOfType<MilestoneWithoutClaim>();
        result.CompletionMilestone.Status.Should().Be(MilestoneStatus.Draft);
        result.CompletionMilestone.GrantApportioned.Should().Be(new GrantApportioned(1000000, 0.01m));
        result.CompletionMilestone.ClaimDate.Should().Be(new ClaimDate(forecastClaimDate));
        result.IsOnlyCompletionMilestone.Should().BeTrue();
    }

    [Theory]
    [InlineData((int)invln_ClaimExternalStatus.Submitted, MilestoneStatus.Submitted)]
    [InlineData((int)invln_ClaimExternalStatus.Approved, MilestoneStatus.Approved)]
    [InlineData((int)invln_ClaimExternalStatus.Paid, MilestoneStatus.Paid)]
    [InlineData((int)invln_ClaimExternalStatus.UnderReview, MilestoneStatus.UnderReview)]
    [InlineData((int)invln_ClaimExternalStatus.Rejected, MilestoneStatus.Rejected)]
    public void ShouldMapSubmittedMilestone_WhenStatusIs(int status, MilestoneStatus expectedStatus)
    {
        // given
        var allocation = new AllocationBasicInfoBuilder().Build();
        var forecastClaimDate = new DateTime(2024, 07, 10, 0, 0, 0, DateTimeKind.Utc);
        var completionClaim = new MilestoneClaimDto
        {
            Type = (int)invln_Milestone.PC,
            Status = status,
            AmountOfGrantApportioned = 1000000,
            PercentageOfGrantApportioned = 1,
            ForecastClaimDate = forecastClaimDate,
        };
        var dto = new PhaseClaimsDto
        {
            AllocationId = allocation.Id.Value,
            Id = "phase-id",
            Name = "My first phase",
            NumberOfHomes = 10,
            NewBuildActivityType = (int)invln_NewBuildActivityType.WorksOnly,
            CompletionMilestone = completionClaim,
        };
        var milestoneAvailabilityStrategy = CreateMilestoneAvailabilityStrategyMock(true);

        // when
        var result = TestCandidate.MapToDomain(dto, allocation, false, milestoneAvailabilityStrategy);

        // then
        result.CompletionMilestone.Should().BeOfType<SubmittedMilestoneClaim>();
        result.CompletionMilestone.Status.Should().Be(expectedStatus);
        result.CompletionMilestone.GrantApportioned.Should().Be(new GrantApportioned(1000000, 0.01m));
        result.CompletionMilestone.ClaimDate.Should().Be(new ClaimDate(forecastClaimDate));
        result.IsOnlyCompletionMilestone.Should().BeTrue();
    }

    private IMilestoneAvailabilityStrategy CreateMilestoneAvailabilityStrategyMock(bool? returnValue = null)
    {
        var mock = new Mock<IMilestoneAvailabilityStrategy>();
        mock.Setup(m =>
            m.OnlyCompletionMilestone(It.IsAny<bool>(), It.IsAny<BuildActivity>())).Returns(returnValue ?? false);
        return mock.Object;
    }
}
