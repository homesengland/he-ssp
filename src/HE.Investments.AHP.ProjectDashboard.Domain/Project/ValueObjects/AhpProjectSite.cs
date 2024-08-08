using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Domain;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;

public class AhpProjectSite : ValueObject
{
    public AhpProjectSite(FrontDoorSiteId? fdSiteId, SiteId id, string name, SiteStatus status, LocalAuthority? localAuthority)
    {
        Id = id;
        FdSiteId = fdSiteId;
        Name = name;
        Status = status;
        LocalAuthority = localAuthority;
    }

    public SiteId Id { get; }

    public FrontDoorSiteId? FdSiteId { get; }

    public string Name { get; }

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
