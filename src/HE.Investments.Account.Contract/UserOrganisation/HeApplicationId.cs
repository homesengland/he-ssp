using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.UserOrganisation;

public record HeApplicationId : StringIdValueObject
{
    private HeApplicationId(string value)
        : base(value)
    {
    }

    public static HeApplicationId From(string value) => new(FromStringToShortGuidAsString(value));

    public static HeApplicationId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public override string ToString()
    {
        return Value;
    }
}
