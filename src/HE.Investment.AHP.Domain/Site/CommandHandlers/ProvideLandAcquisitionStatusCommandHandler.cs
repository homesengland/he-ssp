using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideLandAcquisitionStatusCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideLandAcquisitionStatusCommand>
{
    public ProvideLandAcquisitionStatusCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideLandAcquisitionStatusCommand request, SiteEntity site)
    {
        site.ProvideLandAcquisitionStatus(new LandAcquisitionStatus(request.LandAcquisitionStatus));
    }
}
