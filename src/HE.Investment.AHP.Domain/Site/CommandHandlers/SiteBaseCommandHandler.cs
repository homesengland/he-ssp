using HE.Investment.AHP.Contract.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class SiteBaseCommandHandler
{
    public SiteBaseCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
    {
        SiteRepository = siteRepository;
        AccountUserContext = accountUserContext;
    }

    protected ISiteRepository SiteRepository { get; }

    protected IAccountUserContext AccountUserContext { get; }

    protected async Task<OperationResult> Perform(Func<SiteEntity, Task> action, SiteId siteId, CancellationToken cancellationToken)
    {
        var userAccount = await AccountUserContext.GetSelectedAccount();
        var site = await SiteRepository.GetSite(siteId, userAccount, cancellationToken);
        await action(site);
        await SiteRepository.Save(site, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
