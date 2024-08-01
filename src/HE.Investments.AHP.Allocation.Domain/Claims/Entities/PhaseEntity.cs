using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Contract.Claims.Events;
using HE.Investments.AHP.Allocation.Domain.Allocation.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Domain.Claims.Entities;

public sealed class PhaseEntity : DomainEntity
{
    private readonly ModificationTracker _modificationTracker = new();

    private readonly IOnlyCompletionMilestonePolicy _onlyCompletionMilestonePolicy;

    public PhaseEntity(
        PhaseId id,
        AllocationBasicInfo allocation,
        OrganisationBasicInfo organisation,
        PhaseName name,
        NumberOfHomes numberOfHomes,
        BuildActivityType buildActivityType,
        MilestoneClaimBase? acquisitionMilestone,
        MilestoneClaimBase? startOnSiteMilestone,
        MilestoneClaimBase completionMilestone,
        IOnlyCompletionMilestonePolicy onlyCompletionMilestonePolicy)
    {
        Id = id;
        Allocation = allocation;
        Organisation = organisation;
        Name = name;
        NumberOfHomes = numberOfHomes;
        BuildActivityType = buildActivityType;
        AcquisitionMilestone = acquisitionMilestone;
        StartOnSiteMilestone = startOnSiteMilestone;
        CompletionMilestone = completionMilestone;
        _onlyCompletionMilestonePolicy = onlyCompletionMilestonePolicy;
    }

    public PhaseId Id { get; }

    public AllocationBasicInfo Allocation { get; }

    public OrganisationBasicInfo Organisation { get; }

    public PhaseName Name { get; }

    public NumberOfHomes NumberOfHomes { get; }

    public BuildActivityType BuildActivityType { get; }

    public MilestoneClaimBase? AcquisitionMilestone { get; private set; }

    public MilestoneClaimBase? StartOnSiteMilestone { get; private set; }

    public MilestoneClaimBase CompletionMilestone { get; private set; }

    public bool IsOnlyCompletionMilestone =>
        _onlyCompletionMilestonePolicy.Validate(Organisation.IsUnregisteredBody, new BuildActivity(Allocation.Tenure, type: BuildActivityType));

    public bool IsModified => _modificationTracker.IsModified;

    public MilestoneClaimBase? GetMilestoneClaim(MilestoneType milestoneType)
    {
        if (IsOnlyCompletionMilestone && milestoneType != MilestoneType.Completion)
        {
            return null;
        }

        return milestoneType switch
        {
            MilestoneType.Acquisition => AcquisitionMilestone,
            MilestoneType.StartOnSite => StartOnSiteMilestone,
            MilestoneType.Completion => CompletionMilestone,
            _ => throw new ArgumentOutOfRangeException(nameof(milestoneType), milestoneType, null),
        };
    }

    public void CancelMilestoneClaim(MilestoneType milestoneType)
    {
        var claim = GetMilestoneClaim(milestoneType)
                    ?? throw new NotFoundException(nameof(MilestoneClaim), milestoneType);

        ProvideMilestoneClaim(claim.Cancel());
    }

    public void SubmitMilestoneClaim(MilestoneType milestoneType, DateTime currentDate)
    {
        var claim = GetMilestoneClaim(milestoneType)
                    ?? throw new NotFoundException(nameof(MilestoneClaim), milestoneType);

        ProvideMilestoneClaim(claim.Submit(currentDate));
        Publish(new ClaimHasBeenSubmittedEvent(Allocation.Id, milestoneType));
    }

    public bool CanMilestoneBeClaimed(MilestoneType milestoneType)
    {
        if (IsOnlyCompletionMilestone)
        {
            return milestoneType == MilestoneType.Completion && !CompletionMilestone.IsSubmitted;
        }

        return milestoneType switch
        {
            MilestoneType.Acquisition => AcquisitionMilestone?.IsSubmitted == false,
            MilestoneType.StartOnSite => AcquisitionMilestone?.IsSubmitted == true && StartOnSiteMilestone?.IsSubmitted == false,
            MilestoneType.Completion => !CompletionMilestone.IsSubmitted && (StartOnSiteMilestone.IsNotProvided() || StartOnSiteMilestone!.IsSubmitted),
            _ => false,
        };
    }

    public void ProvideMilestoneClaim(MilestoneClaimBase claim)
    {
        if (!CanMilestoneBeClaimed(claim.Type))
        {
            OperationResult.ThrowValidationError(nameof(MilestoneClaim), $"{claim.Type.GetDescription()} milestone cannot be claimed right now.");
        }

        switch (claim.Type)
        {
            case MilestoneType.Acquisition:
                AcquisitionMilestone = _modificationTracker.Change(AcquisitionMilestone, claim);
                break;
            case MilestoneType.StartOnSite:
                StartOnSiteMilestone = _modificationTracker.Change(StartOnSiteMilestone, claim);
                break;
            case MilestoneType.Completion:
                CompletionMilestone = _modificationTracker.Change(CompletionMilestone, claim);
                break;
            case MilestoneType.Undefined:
            default:
                throw new InvalidOperationException("Cannot provide Unknown claim type.");
        }
    }

    public void ProvideMilestoneClaimAchievementDate(
        MilestoneClaimBase claim,
        Programme.Contract.Programme programme,
        AchievementDate achievementDate,
        DateTime currentDate)
    {
        var previousMilestoneSubmissionDate = claim.Type switch
        {
            MilestoneType.Acquisition => null,
            MilestoneType.StartOnSite => AcquisitionMilestone?.ClaimDate.SubmissionDate,
            MilestoneType.Completion => StartOnSiteMilestone?.ClaimDate.SubmissionDate,
            _ => throw new InvalidOperationException("Cannot provide Unknown claim type."),
        };

        ProvideMilestoneClaim(claim.WithAchievementDate(achievementDate, programme, previousMilestoneSubmissionDate, currentDate));
    }
}
