using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumId : StringIdValueObject
{
    public ConsortiumId(string value)
        : base(value)
    {
    }

    private ConsortiumId()
    {
    }

    public static ConsortiumId New() => new();

    public static ConsortiumId From(string value) => new(FromStringToShortGuidAsString(value));

    public override string ToString()
    {
        return Value;
    }
}
