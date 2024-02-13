using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Application.Repositories.Interfaces;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract.Pagination;
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
        var site = await _siteRepository.GetSite(request.SiteId, userAccount, cancellationToken);
        var applications = await _applicationRepository.GetSiteApplications(request.SiteId, userAccount, request.PaginationRequest, cancellationToken);

        return new SiteDetailsModel(
            site.Id,
            site.Name.Value,
            userAccount.Organisation?.RegisteredCompanyName ?? string.Empty,
            site.LocalAuthority?.Name,
            MapApplicationsPage(applications));
    }

    private static PaginationResult<ApplicationSiteModel> MapApplicationsPage(PaginationResult<ApplicationWithFundingDetails> applications)
    {
        return new PaginationResult<ApplicationSiteModel>(
            applications.Items.Select(MapApplication).ToList(),
            applications.CurrentPage,
            applications.ItemsPerPage,
            applications.TotalItems);
    }

    private static ApplicationSiteModel MapApplication(ApplicationWithFundingDetails application)
    {
        return new ApplicationSiteModel(
            application.ApplicationId,
            application.ApplicationName,
            application.Tenure,
            application.HousesToDeliver,
            application.Status);
    }
}
