using HE.Investment.AHP.Contract.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface ISiteNameExist
{
    Task<bool> IsExist(SiteName name, SiteId exceptSiteId, CancellationToken cancellationToken);
}
