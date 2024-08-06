using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.TestsUtils;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;

public class AllocationEntityTestBuilder : TestObjectBuilder<AllocationEntityTestBuilder, AllocationEntity>
{
    private AllocationEntityTestBuilder(AllocationEntity item)
        : base(item)
    {
    }

    protected override AllocationEntityTestBuilder Builder => this;

    public static AllocationEntityTestBuilder New() => new(new AllocationEntity(
        new AllocationBasicInfoBuilder().Build(),
        new GrantDetails(1000, 500, 500),
        []));

    public AllocationEntityTestBuilder WithListOfPhaseClaims(IList<PhaseEntity> value)
    {
        PrivatePropertySetter.SetPrivateField(Item, "_phases", value);
        return this;
    }
}
