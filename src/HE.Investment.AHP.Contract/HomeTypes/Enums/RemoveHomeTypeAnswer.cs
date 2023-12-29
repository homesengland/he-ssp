using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum RemoveHomeTypeAnswer
{
    Undefined = 0,

    [Description("Yes")]
    Yes,

    [Description("No, I do not want to remove this home type")]
    No,
}
