using System.ComponentModel;

namespace HE.Investment.AHP.Contract.Site;

public enum SiteType
{
    Brownfield,
    Greenfield,
    [Description("No, my site is not a Brownfield or Greenfield site")]
    NotApplicable,
}
