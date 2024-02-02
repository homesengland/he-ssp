using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.Planning;
using HE.Investments.Account.Shared;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.TenderingStatus;

public class ProvideTenderingStatusCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideTenderingStatusCommand>
{
    public ProvideTenderingStatusCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext, ILogger<SiteBaseCommandHandler> logger)
        : base(siteRepository, accountUserContext, logger)
    {
    }

    protected override void Provide(ProvideTenderingStatusCommand request, SiteEntity site)
    {
        site.ProvideTenderingStatusDetails(new TenderingStatusDetails(request.TenderingStatus));
    }
}
