using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum RemoveDeliveryPhaseAnswer
{
    Undefined = 0,

    [Description("Yes")]
    Yes,

    [Description("No, I do not want to remove this delivery phase")]
    No,
}
