using System.ComponentModel;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.Site;

public enum SiteUsingModernMethodsOfConstruction
{
    [Description("Yes, I'm using MMC for all the homes on this site")]
    Yes = 1,
    [Description("Yes, but only for some of the homes on a site")]
    [Hint("You'll be able to provide details against each home type.")]
    OnlyForSomeHomes,
    [Description("No, I'm not using any MMC on this site")]
    No,
}
