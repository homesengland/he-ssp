using System.ComponentModel;

namespace HE.Investments.AHP.Allocation.Contract.Enums;

public enum ChangeDeliveryPlanType
{
    Undefined = 0,

    [Description("I need to view or make a change to the scheme, but I am still delivering all of it")]
    SchemeChange,

    [Description("I can only deliver some of the scheme")]
    DeliverPartially,

    [Description("I can't deliver any of the scheme")]
    NoDelivery,
}
