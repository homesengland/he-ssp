using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum FinishHomeTypesAnswer
{
    Undefined = 0,

    [Description("Yes, I've added all of the home types")]
    Yes,

    [Description("No")]
    No,
}
