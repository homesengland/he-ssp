using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectSite : ValueObject
{
    public AhpProjectSite(
        SiteId id,
        SiteName name,
        SiteStatus siteStatus)
    {
        Id = id;
        Name = name;
        SiteStatus = siteStatus;
    }

    public SiteId Id { get; }

    public SiteName Name { get; }

    public SiteStatus SiteStatus { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return SiteStatus;
    }
}
