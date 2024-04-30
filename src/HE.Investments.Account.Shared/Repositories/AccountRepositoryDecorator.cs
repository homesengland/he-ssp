using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common;
using HE.Investments.Common.Contract;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.Shared.Repositories;

internal sealed class AccountRepositoryDecorator : IAccountRepository
{
    private readonly IFeatureManager _featureManager;

    private readonly Func<AccountCrmRepository> _crmRepositoryFactory;

    private readonly Func<AccountHttpRepository> _httpRepositoryFactory;

    public AccountRepositoryDecorator(IFeatureManager featureManager, Func<AccountCrmRepository> crmRepositoryFactory, Func<AccountHttpRepository> httpRepositoryFactory)
    {
        _featureManager = featureManager;
        _crmRepositoryFactory = crmRepositoryFactory;
        _httpRepositoryFactory = httpRepositoryFactory;
    }

    public async Task<IList<UserAccount>> GetUserAccounts(UserGlobalId userGlobalId, string userEmail)
    {
        var repository = await GetRepository();
        return await repository.GetUserAccounts(userGlobalId, userEmail);
    }

    public async Task<UserProfileDetails> GetProfileDetails(UserGlobalId userGlobalId)
    {
        var repository = await GetRepository();
        return await repository.GetProfileDetails(userGlobalId);
    }

    private async Task<IAccountRepository> GetRepository()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.AccountApiAccess))
        {
            return _httpRepositoryFactory();
        }

        return _crmRepositoryFactory();
    }
}
