using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Contract.Site.ValueObjects;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Utils.Pagination;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetailsModel>
{
    private readonly IAccountUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    private readonly IApplicationRepository _applicationRepository;

    public GetSiteDetailsQueryHandler(IAccountUserContext accountUserContext, ISiteRepository siteRepository, IApplicationRepository applicationRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<SiteDetailsModel> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSite(new SiteId(request.SiteId), userAccount, cancellationToken);
        var applications = await _applicationRepository.GetApplicationsWithFundingDetails(userAccount, new PaginationRequest(1, 100), cancellationToken);

        return new SiteDetailsModel(
            site.Id.Value,
            site.Name.Value,
            userAccount.OrganisationName,
            applications.Items.Select(x => new ApplicationSiteModel(x.ApplicationId.Value, x.ApplicationName, x.Tenure, x.Status)).ToArray());
    }
}
