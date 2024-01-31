using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public class CompleteDeliverySectionModel : DeliveryModelBase
{
    public CompleteDeliverySectionModel(string applicationName)
        : base(applicationName)
    {
    }

    public CompleteDeliverySectionModel()
        : base(string.Empty)
    {
    }

    public IsDeliveryCompleted IsDeliveryCompleted { get; set; }
}
