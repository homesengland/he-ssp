using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.Entities;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Crm;

public sealed class PhaseCrmMapper : IPhaseCrmMapper
{
    private readonly MilestoneStatusMapper _milestoneStatusMapper;

    private readonly MilestoneTypeMapper _milestoneTypeMapper;

    public PhaseCrmMapper(MilestoneStatusMapper milestoneStatusMapper, MilestoneTypeMapper milestoneTypeMapper)
    {
        _milestoneStatusMapper = milestoneStatusMapper;
        _milestoneTypeMapper = milestoneTypeMapper;
    }

    public PhaseEntity MapToDomain(PhaseClaimsDto dto, AllocationBasicInfo allocation)
    {
        return new PhaseEntity(
            PhaseId.From(dto.Id),
            allocation,
            new PhaseName(dto.Name),
            new NumberOfHomes(dto.NumberOfHomes),
            BuildActivityTypeMapper.ToDomain(dto.NewBuildActivityType, dto.RehabBuildActivityType),
            dto.AcquisitionMilestone.IsProvided() ? ToMilestoneClaim(dto.AcquisitionMilestone, MilestoneType.Acquisition) : null,
            dto.StartOnSiteMilestone.IsProvided() ? ToMilestoneClaim(dto.StartOnSiteMilestone, MilestoneType.StartOnSite) : null,
            ToMilestoneClaim(dto.CompletionMilestone, MilestoneType.Completion));
    }

    public PhaseClaimsDto MapToDto(PhaseEntity entity)
    {
        var (newBuildActivityType, rehabBuildActivityType) = BuildActivityTypeMapper.ToDto(entity.BuildActivityType);

        return new PhaseClaimsDto
        {
            Id = entity.Id.ToGuidAsString(),
            AllocationId = entity.Allocation.Id.ToGuidAsString(),
            Name = entity.Name.Value,
            NumberOfHomes = entity.NumberOfHomes.Value,
            NewBuildActivityType = newBuildActivityType,
            RehabBuildActivityType = rehabBuildActivityType,
            AcquisitionMilestone = entity.AcquisitionMilestone != null ? ToMilestoneClaimDto(entity.AcquisitionMilestone) : null,
            StartOnSiteMilestone = entity.StartOnSiteMilestone != null ? ToMilestoneClaimDto(entity.StartOnSiteMilestone) : null,
            CompletionMilestone = ToMilestoneClaimDto(entity.CompletionMilestone),
        };
    }

    private MilestoneClaimDto? ToMilestoneClaimDto(MilestoneClaimBase milestoneClaim)
    {
        return milestoneClaim is MilestoneWithoutClaim
            ? null
            : new MilestoneClaimDto
            {
                Type = _milestoneTypeMapper.ToDto(milestoneClaim.Type)!.Value,
                Status = _milestoneStatusMapper.ToDto(milestoneClaim.Status)!.Value,
                AmountOfGrantApportioned = milestoneClaim.GrantApportioned.Amount,
                PercentageOfGrantApportioned = milestoneClaim.GrantApportioned.Percentage * 100m,
                ForecastClaimDate = milestoneClaim.ClaimDate.ForecastClaimDate,
                AchievementDate = milestoneClaim.ClaimDate.AchievementDate?.Value,
                SubmissionDate = DateTimeExtensions.FromDateDetails(milestoneClaim.ClaimDate.SubmissionDate),
                CostIncurred = milestoneClaim.CostsIncurred,
                IsConfirmed = milestoneClaim.IsConfirmed,
            };
    }

    private MilestoneClaimBase ToMilestoneClaim(MilestoneClaimDto dto, MilestoneType type)
    {
        var grantApportioned = new GrantApportioned(dto.AmountOfGrantApportioned, dto.PercentageOfGrantApportioned / 100m);
        if (dto.Status == 0)
        {
            return new MilestoneWithoutClaim(type, grantApportioned, dto.ForecastClaimDate);
        }

        var status = _milestoneStatusMapper.ToDomain(dto.Status)!.Value;
        var claimDate = new ClaimDate(
            dto.ForecastClaimDate,
            dto.AchievementDate.IsProvided() ? new AchievementDate(dto.AchievementDate) : null,
            DateDetails.FromDateTime(dto.SubmissionDate));

        return status >= MilestoneStatus.Submitted
            ? new SubmittedMilestoneClaim(type, status, grantApportioned, claimDate, dto.CostIncurred, dto.IsConfirmed)
            : new DraftMilestoneClaim(type, grantApportioned, claimDate, dto.CostIncurred, dto.IsConfirmed);
    }
}
