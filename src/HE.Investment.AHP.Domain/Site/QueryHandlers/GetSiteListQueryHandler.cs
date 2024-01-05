using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSiteListQueryHandler : IRequestHandler<GetSiteListQuery, SitesListModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    public GetSiteListQueryHandler(IAccountUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<SitesListModel> Handle(GetSiteListQuery request, CancellationToken cancellationToken)
    {
        var selectedAccount = await _accountUserContext.GetSelectedAccount();
        var sites = await _siteRepository.GetSites(selectedAccount, cancellationToken);

        return new SitesListModel(
            selectedAccount.OrganisationName,
            sites.Select(x => new SiteBasicModel(x.Id.Value, x.Name.Value, x.LocalAuthority, x.Status)).ToArray());
    }
}
