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

    public static HomeTypeId From(string value) => new(FromStringToShortGuidAsString(value));

    public static HomeTypeId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public override string ToString()
    {
        return Value;
    }
}
