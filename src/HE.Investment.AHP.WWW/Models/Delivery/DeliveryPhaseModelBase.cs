namespace HE.Investment.AHP.WWW.Models.Delivery;

public class DeliveryPhaseModelBase : DeliveryModelBase
{
    public DeliveryPhaseModelBase(string applicationName, string deliveryPhaseName)
        : base(applicationName)
    {
        DeliveryPhaseName = deliveryPhaseName;
    }

    public string DeliveryPhaseName { get; set; }
}
