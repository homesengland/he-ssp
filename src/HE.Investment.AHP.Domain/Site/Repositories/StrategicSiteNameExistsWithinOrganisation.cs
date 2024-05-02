using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.Account.Shared.User;

namespace HE.Investment.AHP.Domain.Site.Repositories;

public class StrategicSiteNameExistsWithinOrganisation : IStrategicSiteNameExists
{
    private readonly ISiteRepository _repository;

    private readonly UserAccount _userAccount;

    public StrategicSiteNameExistsWithinOrganisation(ISiteRepository repository, UserAccount userAccount)
    {
        _repository = repository;
        _userAccount = userAccount;
    }

    public async Task<bool> IsExist(StrategicSiteName name, CancellationToken cancellationToken)
    {
        return await _repository.IsExist(name, _userAccount, cancellationToken);
    }
}
