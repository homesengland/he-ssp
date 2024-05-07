using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;

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

    public string ToGuidAsString() => Value.ToGuidAsString();

    public override string ToString()
    {
        return Value;
    }
}
