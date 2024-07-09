using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Contract;

public record AllocationId : StringIdValueObject
{
    public AllocationId(string value)
        : base(value)
    {
    }

    private AllocationId()
    {
    }

    public static AllocationId New() => new();

    public static AllocationId From(string value) => new(FromStringToShortGuidAsString(value));

    public static AllocationId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
