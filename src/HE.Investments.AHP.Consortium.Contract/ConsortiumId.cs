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

    public static ConsortiumId From(string value) => new(FromStringToShortGuidAsString(value));

    public static ConsortiumId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public override string ToString()
    {
        return Value;
    }
}
