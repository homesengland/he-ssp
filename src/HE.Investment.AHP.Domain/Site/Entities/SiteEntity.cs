using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Common.Contract.Validators;

namespace HE.Investment.AHP.Domain.Site.Entities;

public class SiteEntity
{
    public SiteEntity(SiteId id, SiteName name, Section106Entity? section106 = null)
    {
        Id = id;
        Name = name;
        Status = SiteStatus.NotReady;
        Section106 = section106;
    }

    public SiteEntity()
    {
        Id = SiteId.New();
        Name = new SiteName("New Site");
        Status = SiteStatus.NotReady;
    }

    public SiteId Id { get; set; }

    public SiteName Name { get; private set; }

    public Section106Entity? Section106 { get; private set; }

    public string? LocalAuthority { get; }

    public SiteStatus Status { get; }

    public async Task ProvideName(SiteName siteName, ISiteNameExist siteNameExist, CancellationToken cancellationToken)
    {
        if (await siteNameExist.IsExist(siteName, Id, cancellationToken))
        {
            OperationResult.New()
                .AddValidationError(nameof(Name), "There is already a site with this name. Enter a different name")
                .CheckErrors();
        }

        Name = siteName;
    }

    public void ProvideSection106(Section106Entity section106Entity)
    {
        Section106 = section106Entity;
    }
}
