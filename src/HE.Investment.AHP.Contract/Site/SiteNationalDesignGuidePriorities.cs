using HE.Investment.AHP.Contract.Site.Enums;

namespace HE.Investment.AHP.Contract.Site;

public class SiteNationalDesignGuidePriorities
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public IList<NationalDesignGuidePriority> Priorities { get; set; }
}
