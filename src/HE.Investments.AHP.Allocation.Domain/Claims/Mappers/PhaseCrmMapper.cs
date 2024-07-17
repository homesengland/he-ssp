using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Extensions;
using MilestoneClaim = HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects.MilestoneClaim;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Mappers;

public class PhaseCrmMapper : IPhaseCrmMapper
{
    public PhaseEntity MapToDomain(PhaseClaimsDto dto, AllocationBasicInfo allocation)
    {
        return new PhaseEntity(
            PhaseId.From(dto.Id),
            allocation,
            new PhaseName(dto.Name),
            new NumberOfHomes(dto.NumberOfHomes),
            new BuildActivity(MapBuildActivityType(dto.BuildActivityType)),
            dto.AcquisitionMilestone.IsProvided() ? ToMilestoneClaim(dto.AcquisitionMilestone, MilestoneType.Acquisition) : null,
            dto.StartOnSiteMilestone.IsProvided() ? ToMilestoneClaim(dto.StartOnSiteMilestone, MilestoneType.StartOnSite) : null,
            ToMilestoneClaim(dto.CompletionMilestone, MilestoneType.Completion));
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

    private static MilestoneClaimDto ToMilestoneClaimDto(MilestoneClaim milestoneClaim)
    {
        return new MilestoneClaimDto
        {
            Type = (int)milestoneClaim.Type,
            Status = (int)milestoneClaim.Status,
            AmountOfGrantApportioned = milestoneClaim.GrantApportioned.Amount,
            PercentageOfGrantApportioned = milestoneClaim.GrantApportioned.Percentage,
            ForecastClaimDate = milestoneClaim.ClaimDate.ForecastClaimDate,
            AchivmentDate = milestoneClaim.ClaimDate.ActualClaimDate,
            CostIncurred = milestoneClaim.CostIncurred,
            IsConfirmed = milestoneClaim.IsConfirmed,
        };
    }

    private static MilestoneClaim ToMilestoneClaim(MilestoneClaimDto dto, MilestoneType type)
    {
        return new MilestoneClaim(
            type,
            MapMilestoneStatus(dto.Status),
            new GrantApportioned(dto.AmountOfGrantApportioned, dto.PercentageOfGrantApportioned / 100m),
            new ClaimDate(dto.ForecastClaimDate, dto.AchivmentDate),
            dto.CostIncurred,
            dto.IsConfirmed);
    }

    private static BuildActivityType MapBuildActivityType(int buildActivityType)
    {
        return buildActivityType switch
        {
            (int)invln_NewBuildActivityType.Regeneration => BuildActivityType.Regeneration,
            _ => BuildActivityType.WorksOnly,

            // _ => throw new ArgumentOutOfRangeException(nameof(buildActivityType), status, null), // todo: map build activity
        };
    }

    private static MilestoneStatus MapMilestoneStatus(int milestoneStatus)
    {
        return milestoneStatus switch
        {
            // (int)invln_MilestoneStatus.Completed => MilestoneStatus.Completed,
            _ => MilestoneStatus.Draft,

            // _ => throw new ArgumentOutOfRangeException(nameof(milestoneStatus), status, null), // todo: map milestone status
        };
    }
}
