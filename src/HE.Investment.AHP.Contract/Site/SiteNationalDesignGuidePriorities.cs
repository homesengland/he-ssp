using HE.Investment.AHP.Contract.Site.Enums;

namespace HE.Investment.AHP.Contract.Site;

public class SiteNationalDesignGuidePriorities
{
    public SiteId? Id { get; set; }

    public string? Name { get; set; }

    public IList<NationalDesignGuidePriority> Priorities { get; set; }
}
