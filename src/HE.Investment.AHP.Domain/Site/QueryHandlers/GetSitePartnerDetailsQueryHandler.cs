using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSitePartnerDetailsQueryHandler : IRequestHandler<GetSitePartnerDetailsQuery, SitePartnerDetailsModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    public GetSitePartnerDetailsQueryHandler(IAccountUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<SitePartnerDetailsModel> Handle(GetSitePartnerDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSite(SiteId.From(request.SiteId), userAccount, cancellationToken);

        return new SitePartnerDetailsModel(
            site.Id.Value,
            site.Name.Value,
            site.SitePartners.DevelopingPartner?.Id.Value,
            site.SitePartners.OwnerOfTheLand?.Id.Value,
            site.SitePartners.OwnerOfTheHomes?.Id.Value);
    }
}
