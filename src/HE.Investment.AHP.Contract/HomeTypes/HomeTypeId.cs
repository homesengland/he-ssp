using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeTypeId : StringIdValueObject
{
    public HomeTypeId(string value)
        : base(value)
    {
    }

    private HomeTypeId()
    {
    }

    public static HomeTypeId New() => new();

    public static HomeTypeId From(string value) => new(FromStringToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
