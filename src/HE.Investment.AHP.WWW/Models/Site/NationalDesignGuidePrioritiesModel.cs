using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;

namespace HE.Investment.AHP.WWW.Models.Site;

public class NationalDesignGuidePrioritiesModel
{
    public string ProjectId { get; set; }

    public string SiteId { get; set; }

    public string SiteName { get; set; }

    public IList<NationalDesignGuidePriority>? DesignPriorities { get; set; }
}
