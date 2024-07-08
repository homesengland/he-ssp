using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Allocation.Contract.Allocation;

public record AhpAllocationId : StringIdValueObject
{
    public AhpAllocationId(string value)
        : base(value)
    {
    }

    private AhpAllocationId()
    {
    }

    public static AhpAllocationId New() => new();

    public static AhpAllocationId From(string value) => new(FromStringToShortGuidAsString(value));

    public static AhpAllocationId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
