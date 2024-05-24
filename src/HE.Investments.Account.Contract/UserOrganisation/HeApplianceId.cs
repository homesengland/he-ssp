using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Contract.UserOrganisation;

public record HeApplianceId : StringIdValueObject
{
    private HeApplianceId(string value)
        : base(value)
    {
    }

    public static HeApplianceId From(string value) => new(FromStringToShortGuidAsString(value));

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public override string ToString()
    {
        return Value;
    }
}
