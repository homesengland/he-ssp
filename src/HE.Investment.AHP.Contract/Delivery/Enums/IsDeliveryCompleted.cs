using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum IsDeliveryCompleted
{
    Undefied = 0,

    [Description("Yes")]
    Yes,

    [Description("No, I want to add more delivery phases")]
    No,
}
