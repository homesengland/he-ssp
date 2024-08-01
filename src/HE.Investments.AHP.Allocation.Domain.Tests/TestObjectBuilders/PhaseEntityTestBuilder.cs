using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class PhaseEntityTestBuilder : TestObjectBuilder<PhaseEntityTestBuilder, PhaseEntity>
{
    private static Mock<IOnlyCompletionMilestonePolicy> _mockOnlyCompletionMilestonePolicy;

    private PhaseEntityTestBuilder(PhaseEntity item)
        : base(item)
    {
    }

    protected override PhaseEntityTestBuilder Builder => this;

    public static PhaseEntityTestBuilder New(bool? isOnlyCompletionMilestone = null)
    {
        ReturnOnlyCompletionMilestonePolicy(isOnlyCompletionMilestone);
        return new PhaseEntityTestBuilder(new(
            PhaseId.From(GuidTestData.GuidTwo.ToString()),
            new AllocationBasicInfoBuilder().Build(),
            UserAccountTestData.UserAccountOne.Organisation!,
            new PhaseName("Phase"),
            new NumberOfHomes(100),
            BuildActivityType.WorksOnly,
            null,
            null,
            MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).Build(),
            _mockOnlyCompletionMilestonePolicy.Object));
    }

    public PhaseEntityTestBuilder WithId(string value) => SetProperty(x => x.Id, new PhaseId(value));

    public PhaseEntityTestBuilder WithName(string value) => SetProperty(x => x.Name, new PhaseName(value));

    public PhaseEntityTestBuilder WithAcquisitionMilestone(MilestoneClaimBase? value) => SetProperty(x => x.AcquisitionMilestone, value);

    public PhaseEntityTestBuilder WithStartOnSiteMilestone(MilestoneClaimBase? value) => SetProperty(x => x.StartOnSiteMilestone, value);

    public PhaseEntityTestBuilder WithCompletionMilestone(MilestoneClaimBase value) => SetProperty(x => x.CompletionMilestone, value);

    private static void ReturnOnlyCompletionMilestonePolicy(bool? returnValue)
    {
        _mockOnlyCompletionMilestonePolicy = new Mock<IOnlyCompletionMilestonePolicy>();
        _mockOnlyCompletionMilestonePolicy
            .Setup(x => x.Validate(It.IsAny<bool>(), It.IsAny<BuildActivity>())).Returns(returnValue ?? false);
    }
}
