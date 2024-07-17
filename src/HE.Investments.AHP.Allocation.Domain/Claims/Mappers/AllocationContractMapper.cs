using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public sealed class AllocationContractMapper : IAllocationContractMapper
{
    private readonly IPhaseContractMapper _phaseContractMapper;

    public AllocationContractMapper(IPhaseContractMapper phaseContractMapper)
    {
        _phaseContractMapper = phaseContractMapper;
    }

    public AllocationDetails Map(AllocationEntity allocation, PaginationRequest paginationRequest)
    {
        var allocationBasicInfo = new AllocationBasicInfo(
            allocation.Id,
            allocation.Name.Value,
            allocation.ReferenceNumber.Value,
            allocation.LocalAuthority.Name,
            allocation.Programme.ShortName,
            allocation.Tenure.Value);
        var phases = allocation.ListOfPhaseClaims.Select(_phaseContractMapper.Map).ToList();

        var startIndex = (paginationRequest.Page - 1) * paginationRequest.ItemsPerPage;
        var filteredPhases = phases
            .Skip(startIndex)
            .Take(paginationRequest.ItemsPerPage)
            .ToList();

        return new AllocationDetails(
            allocationBasicInfo,
            MapGrantDetails(allocation.GrantDetails),
            new PaginationResult<Phase>(filteredPhases, paginationRequest.Page, paginationRequest.ItemsPerPage, allocation.ListOfPhaseClaims.Count));
    }

    private static GrantDetails MapGrantDetails(Allocation.ValueObjects.GrantDetails grantDetails)
    {
        return new GrantDetails(
            grantDetails.TotalGrantAllocated,
            grantDetails.AmountPaid,
            grantDetails.AmountRemaining);
    }
}
