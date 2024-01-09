using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideNameCommandHandler : SiteBaseCommandHandler, IRequestHandler<ProvideNameCommand, OperationResult>
{
    public ProvideNameCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideNameCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            async site =>
            {
                var siteName = new SiteName(request.Name);
                await site.ProvideName(siteName, SiteRepository, cancellationToken);
            },
            request.SiteId.IsProvided() ? new SiteId(request.SiteId!) : SiteId.New(),
            cancellationToken);
    }
}
