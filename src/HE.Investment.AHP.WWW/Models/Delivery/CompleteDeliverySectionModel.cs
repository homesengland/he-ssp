using HE.Investment.AHP.Contract.Common.Enums;

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

    public IsSectionCompleted IsSectionCompleted { get; set; }
}
