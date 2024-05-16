extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Domain;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class ProjectSite : ValueObject
{
    public ProjectSite(SiteId id, SiteName name, SiteStatus status, LocalAuthority? localAuthority)
    {
        Id = id;
        Name = name;
        Status = status;
        LocalAuthority = localAuthority;
    }

    public SiteId Id { get; }

    public SiteName Name { get; }

    public SiteStatus Status { get; }

    public LocalAuthority? LocalAuthority { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Status;
        yield return LocalAuthority;
    }
}
