using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.ProjectDashboard.Contract.Site;
using HE.Investments.AHP.ProjectDashboard.Contract.Site.Queries;
using HE.Investments.AHP.ProjectDashboard.Domain.Site.Repositories;
using HE.Investments.AHP.ProjectDashboard.Domain.Site.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetailsModel>
{
    private readonly IConsortiumUserContext _accountUserContext;

    private readonly IMediator _mediator;

    private readonly ISiteAllocationRepository _siteAllocationRepository;

    public GetSiteDetailsQueryHandler(
        IConsortiumUserContext accountUserContext,
        IMediator mediator,
        ISiteAllocationRepository siteAllocationRepository)
    {
        _accountUserContext = accountUserContext;
        _mediator = mediator;
        _siteAllocationRepository = siteAllocationRepository;
    }

    public async Task<SiteDetailsModel> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _mediator.Send(new GetSiteBasicDetailsQuery(request.SiteId.Value), cancellationToken);
        var (applications, allocations) = await _siteAllocationRepository.GetSiteApplicationsAndAllocations(request.SiteId, userAccount, cancellationToken);

        return new SiteDetailsModel(
            site.Id,
            site.FrontDoorProjectId,
            site.Name,
            userAccount.Organisation?.RegisteredCompanyName ?? string.Empty,
            site.LocalAuthorityName,
            applications.Select(MapApplication).ToList(),
            allocations.Select(MapAllocation).ToList());
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

    private static AllocationSiteModel MapAllocation(AllocationSiteDetails application)
    {
        return new AllocationSiteModel(
            AllocationId.From(application.Id.Value),
            application.Name,
            application.Tenure,
            application.NumberOfHomes);
    }
}
