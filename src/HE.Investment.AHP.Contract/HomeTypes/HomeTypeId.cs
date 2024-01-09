using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record HomeTypeId : StringIdValueObject
{
    public HomeTypeId(string id)
        : base(id)
    {
    }

    private HomeTypeId()
    {
    }

    public static HomeTypeId New() => new();

    public override string ToString()
    {
        return Value;
    }
}
