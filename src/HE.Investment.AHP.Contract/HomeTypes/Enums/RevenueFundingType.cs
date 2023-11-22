using System.ComponentModel;

namespace HE.Investment.AHP.Contract.HomeTypes.Enums;

public enum RevenueFundingType
{
    Undefined = 0,

    [Description("Yes, revenue funding is needed and the source has been identified")]
    RevenueFundingNeededAndIdentified,

    [Description("Revenue funding is needed but the source has not been identified")]
    RevenueFundingNeededButNotIdentified,

    [Description("No, revenue funding is not needed")]
    RevenueFundingNotNeeded,
}
