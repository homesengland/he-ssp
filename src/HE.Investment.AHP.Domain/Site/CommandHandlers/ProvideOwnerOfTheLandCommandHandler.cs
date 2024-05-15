extern alias Org;

using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using Org::HE.Investments.Organisation.ValueObjects;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public class ProvideOwnerOfTheLandCommandHandler(ISiteRepository siteRepository, IAccountUserContext accountUserContext)
    : ProvideSiteDetailsBaseCommandHandler<ProvideOwnerOfTheLandCommand>(siteRepository, accountUserContext)
{
    protected override void Provide(ProvideOwnerOfTheLandCommand request, SiteEntity site)
    {
        var sitePartners = site.SitePartners.WithOwnerOfTheLand(new InvestmentsOrganisation(request.OrganisationId, string.Empty), request.IsConfirmed);
        site.ProvideSitePartners(sitePartners);
    }
}
