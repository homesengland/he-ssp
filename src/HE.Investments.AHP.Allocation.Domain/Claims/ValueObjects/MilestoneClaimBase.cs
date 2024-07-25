using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public abstract class MilestoneClaimBase : ValueObject
{
    protected readonly ModificationTracker _modificationTracker = new();

    protected MilestoneClaimBase(
        MilestoneType type,
        MilestoneStatus status,
        GrantApportioned grantApportioned,
        ClaimDate claimDate,
        bool? costsIncurred,
        bool? isConfirmed)
    {
        if (type != MilestoneType.Acquisition && costsIncurred.IsProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(CostsIncurred),
                "Costs incurred can be provided only for Acquisition milestone");
        }

        Type = type;
        Status = status;
        GrantApportioned = grantApportioned;
        ClaimDate = claimDate;
        CostsIncurred = costsIncurred;
        IsConfirmed = isConfirmed;
    }

    public bool IsModified => _modificationTracker.IsModified;

    public MilestoneType Type { get; }

    public MilestoneStatus Status { get; }

    public GrantApportioned GrantApportioned { get; }

    public ClaimDate ClaimDate { get; }

    public bool? CostsIncurred { get; }

    public bool? IsConfirmed { get; }

    public abstract bool IsSubmitted { get; }

    public MilestoneDueStatus CalculateDueStatus(DateTime today)
    {
        if (ClaimDate.ForecastClaimDate.Date.IsAfter(today.AddDays(14)))
        {
            return MilestoneDueStatus.Undefined;
        }

        if (ClaimDate.ForecastClaimDate.Date.IsAfter(today.AddDays(6)))
        {
            return MilestoneDueStatus.DueSoon;
        }

        if (ClaimDate.ForecastClaimDate.Date.IsAfter(today.AddDays(-7)))
        {
            return MilestoneDueStatus.Due;
        }

        return MilestoneDueStatus.Overdue;
    }

    public abstract MilestoneClaimBase WithAchievementDate(
        AchievementDate achievementDate,
        Programme.Contract.Programme programme,
        DateDetails? previousSubmissionDate,
        DateTime currentDate);

    public abstract MilestoneClaimBase WithCostsIncurred(bool? costsIncurred);

    public abstract MilestoneClaimBase WithConfirmation(bool? isConfirmed);

    public abstract MilestoneWithoutClaim Cancel();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return Status;
        yield return GrantApportioned;
        yield return ClaimDate;
        yield return CostsIncurred;
        yield return IsConfirmed;
    }
}
