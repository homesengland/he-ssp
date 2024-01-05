using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public class RemoveDeliveryPhaseModel : DeliveryPhaseModelBase
{
    public RemoveDeliveryPhaseModel(string applicationName, string deliveryPhaseName)
        : base(applicationName, deliveryPhaseName)
    {
    }

    public RemoveDeliveryPhaseModel()
        : base(string.Empty, string.Empty)
    {
    }

    public RemoveDeliveryPhaseAnswer RemoveDeliveryPhaseAnswer { get; set; }
}
