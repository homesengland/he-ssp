using System.Globalization;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public class ClaimsContractMapper : IClaimsContractMapper
{
    public AllocationDetails Map(AllocationEntity allocation, PaginationRequest paginationRequest)
    {
        var allocationBasicInfo = new AllocationBasicInfo(
            allocation.Id,
            allocation.Name.Value,
            allocation.ReferenceNumber.Value,
            allocation.LocalAuthority.Name,
            allocation.Programme.ShortName,
            allocation.Tenure.Value);
        var phases = allocation.ListOfPhaseClaims.Select(Map).ToList();

        var startIndex = (paginationRequest.Page - 1) * paginationRequest.ItemsPerPage;
        var filteredPhases = phases
            .Skip(startIndex)
            .Take(paginationRequest.ItemsPerPage)
            .ToList();

        return new AllocationDetails(
            allocationBasicInfo,
            Map(allocation.GrantDetails),
            new PaginationResult<Phase>(filteredPhases, paginationRequest.Page, paginationRequest.ItemsPerPage, allocation.ListOfPhaseClaims.Count));
    }

    public Phase Map(PhaseEntity phase)
    {
        var milestones = new[]
            {
                Map(MilestoneType.Acquisition, phase.AcquisitionMilestone),
                Map(MilestoneType.StartOnSite, phase.StartOnSiteMilestone),
                Map(MilestoneType.Completion, phase.CompletionMilestone),
            }
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();

        return new Phase(
            phase.Id,
            phase.Name.Value,
            Map(phase.Allocation),
            phase.NumberOfHomes.Value.ToString(CultureInfo.InvariantCulture),
            phase.BuildActivityType.Value,
            milestones);
    }

    public MilestoneClaim? Map(MilestoneType milestoneType, Domain.Claims.ValueObjects.MilestoneClaim? milestoneClaim)
    {
        if (milestoneClaim.IsNotProvided())
        {
            return null;
        }

        return new MilestoneClaim(
            milestoneType,
            MilestoneStatus.Due, // TODO: AB#102109 Calculate display status
            milestoneClaim!.GrantApportioned.Amount,
            milestoneClaim.GrantApportioned.Percentage,
            DateDetails.FromDateTime(milestoneClaim.ClaimDate.ForecastClaimDate)!,
            DateDetails.FromDateTime(milestoneClaim.ClaimDate.ActualClaimDate),
            null, // TODO: AB#85082 Map Submission date when available
            false); // TODO: AB#102144 Calculate whether can be claimed
    }

    private AllocationBasicInfo Map(Allocation.ValueObjects.AllocationBasicInfo allocationBasicInfo)
    {
        return new(
            allocationBasicInfo.Id,
            allocationBasicInfo.Name,
            allocationBasicInfo.ReferenceNumber,
            allocationBasicInfo.LocalAuthority,
            allocationBasicInfo.Programme.ShortName,
            allocationBasicInfo.Tenure);
    }

    private GrantDetails Map(Allocation.ValueObjects.GrantDetails grantDetails)
    {
        return new GrantDetails(
            grantDetails.TotalGrantAllocated,
            grantDetails.AmountPaid,
            grantDetails.AmountRemaining);
    }
}
