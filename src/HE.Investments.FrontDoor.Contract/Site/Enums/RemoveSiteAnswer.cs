using System.ComponentModel;

namespace HE.Investments.FrontDoor.Contract.Site.Enums;

public enum RemoveSiteAnswer
{
    Undefined = 0,

    [Description("Yes")]
    Yes,

    [Description("No, I do not want to remove this site")]
    No,
}
