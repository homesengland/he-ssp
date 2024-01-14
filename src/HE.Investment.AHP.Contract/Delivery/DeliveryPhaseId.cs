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

    public override string ToString()
    {
        return Value;
    }
}
