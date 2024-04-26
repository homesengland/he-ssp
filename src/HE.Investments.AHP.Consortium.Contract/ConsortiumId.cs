using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Contract;

public record ConsortiumId : StringIdValueObject
{
    public ConsortiumId(string id)
        : base(id)
    {
    }

    private ConsortiumId()
    {
    }

    public static ConsortiumId New() => new();

    public override string ToString()
    {
        return Value;
    }
}
