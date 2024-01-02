using HE.Investment.AHP.Contract.Site;

namespace HE.Investment.AHP.Domain.Site.Entities;

public class SiteEntity
{
    public SiteEntity(string id, string name)
    {
        Id = id;
        Name = name;
        Status = SiteStatus.NotReady;
    }

    public string Id { get; }

    public string Name { get; }

    public string? LocalAuthority { get; }

    public SiteStatus Status { get; }
}
