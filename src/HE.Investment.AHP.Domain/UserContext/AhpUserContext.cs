using HE.Investment.AHP.Domain.Programme.Config;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Cache;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using MediatR;

namespace HE.Investment.AHP.Domain.UserContext;

internal sealed class AhpUserContext(
    ICacheService cacheService,
    IAccountUserContext accountUserContext,
    IMediator mediator,
    IProgrammeSettings programmeSettings)
    : IAhpUserContext
{
    private readonly ProgrammeId _ahpProgrammeId = new(programmeSettings.AhpProgrammeId);

    private readonly Dictionary<OrganisationId, CachedEntity<AhpConsortiumBasicInfo>> _consortia = new();

    public bool IsLogged => accountUserContext.IsLogged;

    public UserGlobalId UserGlobalId => accountUserContext.UserGlobalId;

    public string Email => accountUserContext.Email;

    public async Task<AhpUserAccount> GetSelectedAccount()
    {
        var userAccount = await accountUserContext.GetSelectedAccount();
        var consortium = await GetCachedAhpConsortiumInfo(userAccount.Organisation?.OrganisationId);

        return new AhpUserAccount(userAccount, consortium);
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

    private async Task<AhpConsortiumBasicInfo> GetCachedAhpConsortiumInfo(OrganisationId? organisationId)
    {
        if (organisationId.IsNotProvided())
        {
            return AhpConsortiumBasicInfo.NoConsortium;
        }

        if (!_consortia.TryGetValue(organisationId!, out var consortiumCachedEntity))
        {
            consortiumCachedEntity = _consortia[organisationId!] = new CachedEntity<AhpConsortiumBasicInfo>(
                cacheService,
                $"ahp-consortium-{organisationId}",
                async () => await GetAhpConsortiumInfo(organisationId!));
        }

        var consortium = await consortiumCachedEntity.GetAsync();

        return consortium!;
    }

    private async Task<AhpConsortiumBasicInfo> GetAhpConsortiumInfo(OrganisationId organisationId)
    {
        var consortium = await mediator.Send(new GetOrganisationConsortiumQuery(organisationId, _ahpProgrammeId));
        return consortium.IsProvided()
            ? new AhpConsortiumBasicInfo(consortium!.Id, organisationId == consortium.LeadPartnerId, consortium.ActiveMembers)
            : AhpConsortiumBasicInfo.NoConsortium;
    }
}
