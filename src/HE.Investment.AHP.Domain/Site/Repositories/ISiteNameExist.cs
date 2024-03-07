using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public interface ISiteNameExist
{
    Task<bool> IsExist(SiteName name, CancellationToken cancellationToken);
}
