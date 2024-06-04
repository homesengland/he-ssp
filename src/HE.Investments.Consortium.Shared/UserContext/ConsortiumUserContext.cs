using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Consortium.Shared.Repositories;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Config;

namespace HE.Investments.Consortium.Shared.UserContext;

internal sealed class ConsortiumUserContext(
    ICacheService cacheService,
    IAccountUserContext accountUserContext,
    IConsortiumRepository consortiumRepository,
    IProgrammeSettings programmeSettings)
    : IConsortiumUserContext
{
    private readonly Dictionary<OrganisationId, CachedEntity<ConsortiumBasicInfo>> _consortia = [];

    public bool IsLogged => accountUserContext.IsLogged;

    public UserGlobalId UserGlobalId => accountUserContext.UserGlobalId;

    public string Email => accountUserContext.Email;

    public async Task<ConsortiumUserAccount> GetSelectedAccount()
    {
        var userAccount = await accountUserContext.GetSelectedAccount();
        var consortium = await GetCachedAhpConsortiumInfo(userAccount.Organisation?.OrganisationId, userAccount.Role());

        return new ConsortiumUserAccount(userAccount, consortium);
    }

    public async Task<UserProfileDetails> GetProfileDetails()
    {
        return await accountUserContext.GetProfileDetails();
    }

    public async Task RefreshUserData()
    {
        await accountUserContext.RefreshUserData();
    }

    public async Task<bool> IsProfileCompleted()
    {
        return await accountUserContext.IsProfileCompleted();
    }

    public async Task<bool> IsLinkedWithOrganisation()
    {
        return await accountUserContext.IsLinkedWithOrganisation();
    }

    private async Task<ConsortiumBasicInfo> GetCachedAhpConsortiumInfo(OrganisationId? organisationId, UserRole role)
    {
        if (organisationId.IsNotProvided() || role <= UserRole.Limited)
        {
            return ConsortiumBasicInfo.NoConsortium;
        }

        if (!_consortia.TryGetValue(organisationId!, out var consortiumCachedEntity))
        {
            consortiumCachedEntity = _consortia[organisationId!] = new CachedEntity<ConsortiumBasicInfo>(
                cacheService,
                ConsortiumCacheKeys.OrganisationConsortium(organisationId!),
                async () => await consortiumRepository.GetConsortium(organisationId!, ProgrammeId.From(programmeSettings.AhpProgrammeId)));
        }

        var consortium = await consortiumCachedEntity.GetAsync();

        return consortium!;
    }
}
