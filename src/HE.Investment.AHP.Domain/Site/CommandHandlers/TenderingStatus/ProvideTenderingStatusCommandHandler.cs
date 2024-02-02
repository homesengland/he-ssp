using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.TenderingStatus;

public class ProvideTenderingStatusCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideTenderingStatusCommand>
{
    public ProvideTenderingStatusCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideTenderingStatusCommand request, SiteEntity site)
    {
        site.ProvideTenderingStatusDetails(new TenderingStatusDetails(request.TenderingStatus, site.TenderingStatusDetails.ContractorName, site.TenderingStatusDetails.IsSmeContractor));
    }
}
