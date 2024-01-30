using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DeliveryPhaseName : ShortText
{
    public DeliveryPhaseName(string? value)
        : base(value, nameof(DeliveryPhaseName), "the delivery phase name")
    {
    }

    public override string ToString()
    {
        return Value;
    }
}
