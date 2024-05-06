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

    public static ProgrammeId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString() => Value;
}
