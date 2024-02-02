using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investment.AHP.Domain.Site.ValueObjects.TenderingStatus;
using HE.Investments.Account.Shared;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers.TenderingStatus;

public class ProvideContractorDetailsCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideContractorDetailsCommand>
{
    public ProvideContractorDetailsCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
        : base(siteRepository, accountUserContext)
    {
    }

    protected override void Provide(ProvideContractorDetailsCommand request, SiteEntity site)
    {
        var contractorName = new ContractorName(request.ContractorName);
        site.ProvideTenderingStatusDetails(new TenderingStatusDetails(site.TenderingStatusDetails.TenderingStatus, contractorName, request.IsSmeContractor));
    }
}
