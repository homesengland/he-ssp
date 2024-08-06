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
        var startIndex = (paginationRequest.Page - 1) * paginationRequest.ItemsPerPage;
        var filteredPhases = allocation.ListOfPhaseClaims
            .Select(_phaseContractMapper.Map)
            .Skip(startIndex)
            .Take(paginationRequest.ItemsPerPage)
            .ToList();

        return new AllocationDetails(
            MapAllocationBasicInfo(allocation.BasicInfo),
            MapGrantDetails(allocation.GrantDetails),
            new PaginationResult<Phase>(filteredPhases, paginationRequest.Page, paginationRequest.ItemsPerPage, allocation.ListOfPhaseClaims.Count()));
    }

    private static AllocationBasicInfo MapAllocationBasicInfo(
        HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects.AllocationBasicInfo allocationBasicInfo)
    {
        return new AllocationBasicInfo(
            allocationBasicInfo.Id,
            allocationBasicInfo.Name.Value,
            allocationBasicInfo.ReferenceNumber.Value,
            allocationBasicInfo.LocalAuthority.Name,
            allocationBasicInfo.Programme.ShortName,
            allocationBasicInfo.Tenure);
    }

    private static GrantDetails MapGrantDetails(Allocation.ValueObjects.GrantDetails grantDetails)
    {
        return new GrantDetails(
            grantDetails.TotalGrantAllocated,
            grantDetails.AmountPaid,
            grantDetails.AmountRemaining);
    }
}
