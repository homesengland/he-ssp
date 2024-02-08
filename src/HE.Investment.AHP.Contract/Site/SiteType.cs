using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site;

public enum SiteType
{
    Undefined,
    Brownfield,
    Greenfield,
    [Description("Not applicable")]
    NotApplicable,
}
