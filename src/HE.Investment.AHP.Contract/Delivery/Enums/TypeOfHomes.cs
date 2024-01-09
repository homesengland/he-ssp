using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Delivery.Enums;

public enum TypeOfHomes
{
    Undefined = 0,

    [Description("New build")]
    NewBuild,

    [Description("Rehab")]
    Rehab,
}
