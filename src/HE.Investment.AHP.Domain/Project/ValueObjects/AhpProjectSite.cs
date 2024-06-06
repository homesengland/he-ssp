using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investment.AHP.Domain.Project.ValueObjects;

public class AhpProjectSite : ValueObject
{
    public AhpProjectSite(FrontDoorSiteId? fdSiteId, SiteId id, SiteName name, SiteStatus status, LocalAuthority? localAuthority)
    {
        Id = id;
        FdSiteId = fdSiteId;
        Name = name;
        Status = status;
        LocalAuthority = localAuthority;
    }

    public SiteId Id { get; }

    public FrontDoorSiteId? FdSiteId { get; }

    public SiteName Name { get; }

    public SiteStatus Status { get; }

    public LocalAuthority? LocalAuthority { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Id;
        yield return FdSiteId;
        yield return Name;
        yield return Status;
        yield return LocalAuthority;
    }
}
