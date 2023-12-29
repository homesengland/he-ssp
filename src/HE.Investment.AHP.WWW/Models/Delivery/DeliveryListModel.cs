using HE.Investments.Common.WWW.Models;

namespace HE.Investment.AHP.WWW.Models.Delivery;

public class DeliveryListModel : DeliveryModelBase, IEditableViewModel
{
    public DeliveryListModel(string applicationName)
        : base(applicationName)
    {
    }

    public DeliveryListModel()
        : base(string.Empty)
    {
    }

    public bool IsEditable { get; set; }

    public bool IsReadOnly => !IsEditable;

    public int UnusedHomeTypesCount { get; set; }

    public IList<DeliveryPhaseItemModel> DeliveryPhases { get; set; }
}
