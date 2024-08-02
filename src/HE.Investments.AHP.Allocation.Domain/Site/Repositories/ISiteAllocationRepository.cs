using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.AHP.Allocation.Domain.Site.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.AHP.Allocation.Domain.Site.Repositories;

public interface ISiteAllocationRepository
{
    Task<(IList<ApplicationBasicDetails> Applications, IList<AllocationSiteDetails> Allocations)> GetSiteApplicationsAndAllocations(
        SiteId siteId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken);
}
