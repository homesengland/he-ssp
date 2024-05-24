using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideDevelopingPartnerCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    : ProvideSiteDetailsBaseCommandHandler<ProvideDevelopingPartnerCommand>(siteRepository, accountUserContext)
{
    protected override void Provide(ProvideDevelopingPartnerCommand request, SiteEntity site)
    {
        var sitePartners = site.SitePartners.WithDevelopingPartner(new InvestmentsOrganisation(request.OrganisationId, string.Empty), request.IsConfirmed);
        site.ProvideSitePartners(sitePartners);
    }
}
