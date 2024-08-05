using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Site;
using HE.Investments.AHP.Allocation.Contract.Site.Queries;
using HE.Investments.AHP.Allocation.Domain.Site.Repositories;
using HE.Investments.AHP.Allocation.Domain.Site.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Site.QueryHandlers;

public class GetSiteDetailsQueryHandler : IRequestHandler<GetSiteDetailsQuery, SiteDetailsModel>
{
    private readonly IConsortiumUserContext _accountUserContext;

    private readonly ISiteRepository _siteRepository;

    private readonly ISiteAllocationRepository _siteAllocationRepository;

    public GetSiteDetailsQueryHandler(
        IConsortiumUserContext accountUserContext,
        ISiteRepository siteRepository,
        ISiteAllocationRepository siteAllocationRepository)
    {
        _accountUserContext = accountUserContext;
        _siteRepository = siteRepository;
        _siteAllocationRepository = siteAllocationRepository;
    }

    public async Task<SiteDetailsModel> Handle(GetSiteDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var site = await _siteRepository.GetSiteBasicInfo(request.SiteId, userAccount, cancellationToken);
        var (applications, allocations) = await _siteAllocationRepository.GetSiteApplicationsAndAllocations(request.SiteId, userAccount, cancellationToken);

        return new SiteDetailsModel(
            site.Id,
            site.FrontDoorProjectId ?? FrontDoorProjectId.New(),
            site.Name.Value,
            userAccount.Organisation?.RegisteredCompanyName ?? string.Empty,
            site.LocalAuthority?.Name,
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
