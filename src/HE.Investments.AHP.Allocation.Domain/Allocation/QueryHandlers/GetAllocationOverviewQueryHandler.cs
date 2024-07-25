using HE.Investments.AHP.Allocation.Contract.Overview;
using HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.QueryHandlers;

public class GetAllocationOverviewQueryHandler : IRequestHandler<GetAllocationOverviewQuery, AllocationOverview>
{
    private readonly IAllocationRepository _allocationRepository;

    private readonly IConsortiumUserContext _userContext;

    public GetAllocationOverviewQueryHandler(IAllocationRepository allocationRepository, IConsortiumUserContext userContext)
    {
        _allocationRepository = allocationRepository;
        _userContext = userContext;
    }

    public async Task<AllocationOverview> Handle(GetAllocationOverviewQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _userContext.GetSelectedAccount();
        return await _allocationRepository.GetOverview(request.AllocationId, userAccount, cancellationToken);
    }
}
