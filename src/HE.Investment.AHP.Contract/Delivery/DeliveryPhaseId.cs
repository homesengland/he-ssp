using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Delivery;

public record DeliveryPhaseId : StringIdValueObject
{
    public DeliveryPhaseId(string id)
        : base(id)
    {
    }

    private DeliveryPhaseId()
    {
    }

    public static DeliveryPhaseId New() => new();

    public static DeliveryPhaseId From(string value) => new(FromStringToShortGuidAsString(value));

    public static DeliveryPhaseId From(Guid value) => new(FromGuidToShortGuidAsString(value));

    public string ToGuidAsString() => ShortGuid.ToGuidAsString(Value);

    public override string ToString()
    {
        return Value;
    }
}
