using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideNameCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideNameCommand, OperationResult<SiteId>>
{
    public ProvideNameCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult<SiteId>> Handle(ProvideNameCommand request, CancellationToken cancellationToken)
    {
        return await PerformWithResultReturn(
            async site =>
            {
                var siteName = new SiteName(request.Name);
                await site.ProvideName(siteName, SiteRepository, cancellationToken);
            },
            request.SiteId.IsProvided() ? new SiteId(request.SiteId!) : SiteId.New(),
            cancellationToken);
    }

    protected async Task<OperationResult<SiteId>> PerformWithResultReturn(Func<SiteEntity, Task> action, SiteId siteId, CancellationToken cancellationToken)
    {
        var userAccount = await AccountUserContext.GetSelectedAccount();
        var site = await SiteRepository.GetSite(siteId, userAccount, cancellationToken);
        await action(site);
        var newSiteId = await SiteRepository.Save(site, userAccount, cancellationToken);
        return OperationResult.Success(newSiteId);
    }
}
