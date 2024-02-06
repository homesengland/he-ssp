using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Pagination;
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
        var sitesPage = await _siteRepository.GetSites(selectedAccount, request.PaginationRequest, cancellationToken);

        return new SitesListModel(
            selectedAccount.Organisation?.RegisteredCompanyName ?? string.Empty,
            new PaginationResult<SiteBasicModel>(
                sitesPage.Items.Select(x => new SiteBasicModel(x.Id.Value, x.Name.Value, x.LocalAuthority?.Name, x.Status)).ToList(),
                sitesPage.CurrentPage,
                sitesPage.ItemsPerPage,
                sitesPage.TotalItems));
    }
}
