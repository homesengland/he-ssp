using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSiteBasicDetailsQueryHandler : IRequestHandler<GetSiteBasicDetailsQuery, SiteBasicModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    public GetSiteBasicDetailsQueryHandler(IAccountUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<SiteBasicModel> Handle(GetSiteBasicDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSite(SiteId.From(request.SiteId), userAccount, cancellationToken);

        return new SiteBasicModel(
            site.Id.Value,
            site.Name.Value,
            site.LocalAuthority?.Name,
            site.Status);
    }
}
