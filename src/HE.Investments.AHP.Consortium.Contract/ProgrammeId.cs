using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ProgrammeId : StringIdValueObject
{
    public ProgrammeId(string value)
        : base(value)
    {
    }

    public override string ToString() => Value;
}
