using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;
using MilestoneClaim = HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects.MilestoneClaim;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public class PhaseCrmMapper : IPhaseCrmMapper
{
    public PhaseEntity MapToDomain(PhaseClaimsDto dto)
    {
        return new PhaseEntity(
            PhaseId.From(dto.Id),
            AllocationId.From(dto.AllocationId),
            new PhaseName(dto.Name),
            new NumberOfHomes(dto.NumberOfHomes),
            new BuildActivity(MapBuildActivityType(dto.BuildActivityType)),
            ToMilestoneClaim(dto.CompletionMilestone),
            dto.AcquisitionMilestone.IsProvided() ? ToMilestoneClaim(dto.AcquisitionMilestone) : null,
            dto.StartOnSiteMilestone.IsProvided() ? ToMilestoneClaim(dto.StartOnSiteMilestone) : null);
    }

    public PhaseClaimsDto MapToDto(PhaseEntity entity)
    {
        return new PhaseClaimsDto
        {
            Id = entity.Id.Value,
            Name = entity.Name.Value,
            NumberOfHomes = entity.NumberOfHomes.Value,
            BuildActivityType = (int)entity.BuildActivityType.Value,
            AcquisitionMilestone = entity.AcquisitionMilestone != null ? ToMilestoneClaimDto(entity.AcquisitionMilestone) : null,
            StartOnSiteMilestone = entity.StartOnSiteMilestone != null ? ToMilestoneClaimDto(entity.StartOnSiteMilestone) : null,
            CompletionMilestone = ToMilestoneClaimDto(entity.CompletionMilestone),
        };
    }

    private MilestoneClaimDto ToMilestoneClaimDto(MilestoneClaim milestoneClaim)
    {
        return new MilestoneClaimDto
        {
            Type = (int)milestoneClaim.Type,
            Status = (int)milestoneClaim.Status,
            AmountOfGrantApportioned = milestoneClaim.GrantApportioned.Amount,
            PercentageOfGrantApportioned = milestoneClaim.GrantApportioned.Percentage,
            ForecastClaimDate = milestoneClaim.ClaimDate.ForecastClaimDate,
            ClaimDate = milestoneClaim.ClaimDate.ActualClaimDate,
            CostIncurred = milestoneClaim.CostIncurred,
            IsConfirmed = milestoneClaim.IsConfirmed,
        };
    }

    private MilestoneClaim ToMilestoneClaim(MilestoneClaimDto dto)
    {
        return new MilestoneClaim(
            MapMilestoneType(dto.Type),
            MapMilestoneStatus(dto.Status),
            new GrantApportioned(dto.AmountOfGrantApportioned, dto.PercentageOfGrantApportioned),
            new ClaimDate(dto.ForecastClaimDate, dto.ClaimDate),
            dto.CostIncurred,
            dto.IsConfirmed);
    }

    private BuildActivityType MapBuildActivityType(int buildActivityType)
    {
        return buildActivityType switch
        {
            (int)invln_NewBuildActivityType.Regeneration => BuildActivityType.Regeneration,
            _ => BuildActivityType.WorksOnly,

            // _ => throw new ArgumentOutOfRangeException(nameof(buildActivityType), status, null), // todo: map build activity
        };
    }

    private MilestoneType MapMilestoneType(int milestoneType)
    {
        return milestoneType switch
        {
            // (int)invln_MilestoneType.Acquisition => MilestoneType.Acquisition,
            _ => MilestoneType.Completion,

            // _ => throw new ArgumentOutOfRangeException(nameof(milestoneType), status, null), // todo: map milestone type
        };
    }

    private MilestoneStatus MapMilestoneStatus(int milestoneStatus)
    {
        return milestoneStatus switch
        {
            // (int)invln_MilestoneStatus.Completed => MilestoneStatus.Completed,
            _ => MilestoneStatus.Draft,

            // _ => throw new ArgumentOutOfRangeException(nameof(milestoneStatus), status, null), // todo: map milestone status
        };
    }
}
