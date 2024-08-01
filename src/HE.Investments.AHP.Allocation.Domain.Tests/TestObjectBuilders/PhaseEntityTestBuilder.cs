using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class PhaseEntityTestBuilder : TestObjectBuilder<PhaseEntityTestBuilder, PhaseEntity>
{
    private PhaseEntityTestBuilder(PhaseEntity item)
        : base(item)
    {
    }

    protected override PhaseEntityTestBuilder Builder => this;

    public static PhaseEntityTestBuilder New() => new(new(
        PhaseId.From(GuidTestData.GuidTwo.ToString()),
        new AllocationBasicInfoBuilder().Build(),
        new PhaseName("Phase"),
        new NumberOfHomes(100),
        BuildActivityType.WorksOnly,
        null,
        null,
        MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).Build(),
        false));

    public PhaseEntityTestBuilder WithId(string value) => SetProperty(x => x.Id, new PhaseId(value));

    public PhaseEntityTestBuilder WithName(string value) => SetProperty(x => x.Name, new PhaseName(value));

    public PhaseEntityTestBuilder WithAcquisitionMilestone(MilestoneClaimBase? value) => SetProperty(x => x.AcquisitionMilestone, value);

    public PhaseEntityTestBuilder WithStartOnSiteMilestone(MilestoneClaimBase? value) => SetProperty(x => x.StartOnSiteMilestone, value);

    public PhaseEntityTestBuilder WithCompletionMilestone(MilestoneClaimBase value) => SetProperty(x => x.CompletionMilestone, value);

    public PhaseEntityTestBuilder WithIsOnlyCompletionMilestone(bool value) => SetProperty(x => x.IsOnlyCompletionMilestone, value);
}
