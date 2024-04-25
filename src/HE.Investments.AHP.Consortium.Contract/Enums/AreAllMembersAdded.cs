using System.ComponentModel;

namespace HE.Investments.AHP.Consortium.Contract.Enums;

public enum AreAllMembersAdded
{
    Undefined = 0,
    Yes,
    [Description("No, add another member now")]
    No,
}
