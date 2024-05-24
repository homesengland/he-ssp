using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideOwnerOfTheHomesCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    : ProvideSiteDetailsBaseCommandHandler<ProvideOwnerOfTheHomesCommand>(siteRepository, accountUserContext)
{
    protected override void Provide(ProvideOwnerOfTheHomesCommand request, SiteEntity site)
    {
        var sitePartners = site.SitePartners.WithOwnerOfTheHomes(new InvestmentsOrganisation(request.OrganisationId, string.Empty), request.IsConfirmed);
        site.ProvideSitePartners(sitePartners);
    }
}
