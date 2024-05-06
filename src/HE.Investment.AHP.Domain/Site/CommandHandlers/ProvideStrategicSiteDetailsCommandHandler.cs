using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.StrategicSite;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideStrategicSiteDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideStrategicSiteDetailsCommand>
{
    private readonly ISiteRepository _siteRepository;

    private readonly IAccountUserContext _accountUserContext;

    public ProvideStrategicSiteDetailsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
        _siteRepository = siteRepository;
        _accountUserContext = accountUserContext;
    }

    protected override async Task Provide(ProvideStrategicSiteDetailsCommand request, SiteEntity site, CancellationToken cancellationToken)
    {
        var strategicSite = request.IsStrategicSite.IsProvided() ?
            StrategicSiteDetails.Create(request.IsStrategicSite, request.StrategicSiteName) : null;
        var strategicSiteNameExists = new StrategicSiteNameExistsWithinOrganisation(_siteRepository, await _accountUserContext.GetSelectedAccount());

        await site.ProvideStrategicSiteDetails(strategicSite, strategicSiteNameExists, cancellationToken);
    }
}
