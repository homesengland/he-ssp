using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Contract.Claims;

public record PhaseId : StringIdValueObject
{
    public PhaseId(string value)
        : base(value)
    {
    }

    private PhaseId()
    {
    }

    public static PhaseId New() => new();

    public static PhaseId From(string value) => new(FromStringToShortGuidAsString(value));

    public static PhaseId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
