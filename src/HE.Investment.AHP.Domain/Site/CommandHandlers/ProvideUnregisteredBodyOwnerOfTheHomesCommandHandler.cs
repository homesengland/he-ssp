using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Organisation.Services;

namespace HE.Investment.AHP.Domain.Site.CommandHandlers;

public sealed class ProvideUnregisteredBodyOwnerOfTheHomesCommandHandler : ProvideSiteDetailsBaseCommandHandler<ProvideUnregisteredBodyOwnerOfTheHomesCommand>
{
    private readonly IInvestmentsOrganisationService _organisationService;

    public ProvideUnregisteredBodyOwnerOfTheHomesCommandHandler(
        ISiteRepository siteRepository, IAccountUserContext accountUserContext, IInvestmentsOrganisationService organisationService)
        : base(siteRepository, accountUserContext)
    {
        _organisationService = organisationService;
    }

    protected override async Task Provide(ProvideUnregisteredBodyOwnerOfTheHomesCommand request, SiteEntity site, CancellationToken cancellationToken)
    {
        var organisation = await _organisationService.GetOrganisation(request.OrganisationIdentifier, cancellationToken);
        var sitePartners = site.SitePartners.WithOwnerOfTheHomes(organisation, request.IsConfirmed);

        site.ProvideSitePartners(sitePartners);
    }
}
