using System.ComponentModel;

namespace HE.Investments.FrontDoor.Contract.Site;

public enum AddAnotherSite
{
    Undefined,
    Yes,
    [Description("No, I have added all of my sites")]
    No,
}
