using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DeliveryPhaseName : ShortText
{
    public DeliveryPhaseName(string? value)
        : base(value, nameof(DeliveryPhaseName), "Delivery phase name")
    {
    }
}
