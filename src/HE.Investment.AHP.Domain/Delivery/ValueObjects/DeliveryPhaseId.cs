using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

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
}
