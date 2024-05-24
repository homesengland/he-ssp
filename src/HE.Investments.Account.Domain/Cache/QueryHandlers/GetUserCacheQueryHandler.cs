using HE.Investments.Account.Contract.Cache;
using HE.Investments.Account.Contract.Cache.Queries;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using MediatR;

namespace HE.Investments.Account.Domain.Cache.QueryHandlers;

public class GetUserCacheQueryHandler : IRequestHandler<GetUserCacheQuery, UserCacheModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ICacheService _cacheService;

    public GetUserCacheQueryHandler(
        IAccountUserContext accountUserContext,
        ICacheService cacheService)
    {
        _accountUserContext = accountUserContext;
        _cacheService = cacheService;
    }

    public async Task<UserCacheModel> Handle(GetUserCacheQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        var userAccounts = _cacheService.GetValue<IList<UserAccount>>(AccountCacheKeys.UserAccounts(account.UserGlobalId.ToString()));
        var userProfileDetails = _cacheService.GetValue<UserProfileDetails>(AccountCacheKeys.ProfileDetails(account.UserGlobalId.ToString()));
        var organisationConsortium = _cacheService.GetValue<AhpConsortiumBasicInfo>(AccountCacheKeys.OrganisationConsortium(account.SelectedOrganisationId()));

        return new UserCacheModel(userAccounts, userProfileDetails, organisationConsortium);
    }
}
