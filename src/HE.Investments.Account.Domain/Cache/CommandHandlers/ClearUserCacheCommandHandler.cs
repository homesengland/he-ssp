using HE.Investments.Account.Contract.Cache.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using MediatR;

namespace HE.Investments.Account.Domain.Cache.CommandHandlers;

public class ClearUserCacheCommandHandler : IRequestHandler<ClearUserCacheCommand, OperationResult>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ICacheService _cacheService;

    public ClearUserCacheCommandHandler(IAccountUserContext accountUserContext, ICacheService cacheService)
    {
        _accountUserContext = accountUserContext;
        _cacheService = cacheService;
    }

    public async Task<OperationResult> Handle(ClearUserCacheCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();

        await _cacheService.DeleteAsync(AccountCacheKeys.UserAccounts(account.UserGlobalId.ToString()));
        await _cacheService.DeleteAsync(AccountCacheKeys.ProfileDetails(account.UserGlobalId.ToString()));
        await _cacheService.DeleteAsync(AccountCacheKeys.OrganisationConsortium(account.SelectedOrganisationId()));

        return OperationResult.Success();
    }
}
