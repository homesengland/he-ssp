using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeTypeId : StringIdValueObject
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

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
