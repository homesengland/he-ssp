using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Consortium.Contract;

public record ProgrammeId : StringIdValueObject
{
    public ProgrammeId(string value)
        : base(value)
    {
    }

    public static ProgrammeId From(string value) => new(FromStringToShortGuidAsString(value));

    public override string ToString() => Value;
}
