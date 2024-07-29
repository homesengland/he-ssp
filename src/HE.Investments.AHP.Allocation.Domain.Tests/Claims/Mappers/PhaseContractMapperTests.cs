using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Mappers;

public class PhaseContractMapperTests : TestBase<PhaseContractMapper>
{
    [Fact]
    public void ShouldExcludeNullMilestones_WhenMilestonesAreMissing()
    {
        // given
        var phase = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(null)
            .WithStartOnSiteMilestone(null)
            .WithCompletionMilestone(MilestoneClaimTestBuilder.Draft().Build())
            .Build();

        var claimMapper = CreateAndRegisterDependencyMock<IMilestoneClaimContractMapper>();
        claimMapper.Setup(x => x.Map(MilestoneType.Completion, phase, It.IsAny<DateTime>()))
            .Returns(new MilestoneClaim(MilestoneType.Completion, MilestoneStatus.Due, 0, 0, DateDetails.Empty(), null, null, false, null, null, true));

        // when
        var result = TestCandidate.Map(phase);

        // then
        result.MilestoneClaims.Count.Should().Be(1);
        result.MilestoneClaims.Should().NotContain(x => x.Type == MilestoneType.Acquisition);
        result.MilestoneClaims.Should().NotContain(x => x.Type == MilestoneType.StartOnSite);
        result.MilestoneClaims.Should().Contain(x => x.Type == MilestoneType.Completion);
    }
}
