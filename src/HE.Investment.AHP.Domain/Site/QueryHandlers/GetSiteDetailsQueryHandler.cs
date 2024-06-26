using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetailsModel>
{
    private readonly IConsortiumUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    public GetSiteDetailsQueryHandler(IConsortiumUserContext accountUserContext, ISiteRepository siteRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
    }

    public async Task<SiteDetailsModel> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSite(request.SiteId, userAccount, cancellationToken);
        var applications = await _siteRepository.GetSiteApplications(request.SiteId, userAccount, request.PaginationRequest, cancellationToken);

        return new SiteDetailsModel(
            site.Id,
            site.FrontDoorProjectId,
            site.Name.Value,
            userAccount.Organisation?.RegisteredCompanyName ?? string.Empty,
            site.LocalAuthority?.Name,
            MapApplicationsPage(applications));
    }

    private static PaginationResult<ApplicationSiteModel> MapApplicationsPage(PaginationResult<ApplicationBasicDetails> applications)
    {
        return new PaginationResult<ApplicationSiteModel>(
            applications.Items.Select(MapApplication).ToList(),
            applications.CurrentPage,
            applications.ItemsPerPage,
            applications.TotalItems);
    }

    private static ApplicationSiteModel MapApplication(ApplicationBasicDetails application)
    {
        return new ApplicationSiteModel(
            application.Id,
            application.Name,
            application.Tenure,
            application.Unit,
            application.Status);
    }
}
