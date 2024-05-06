using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface IStrategicSiteNameExists
{
    Task<bool> IsExist(StrategicSiteName name, CancellationToken cancellationToken);
}
