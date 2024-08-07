using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public abstract class MilestoneClaimBase : ValueObject
{
    protected MilestoneClaimBase(
        MilestoneType type,
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
        GrantApportioned = grantApportioned;
        ClaimDate = claimDate;
        CostsIncurred = costsIncurred;
        IsConfirmed = isConfirmed;
    }

    public MilestoneType Type { get; }

    public abstract MilestoneStatus Status { get; }

    public GrantApportioned GrantApportioned { get; }

    public ClaimDate ClaimDate { get; }

    public bool? CostsIncurred { get; }

    public bool? IsConfirmed { get; }

    public abstract bool IsEditable { get; }

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
        DateTime? previousSubmissionDate,
        DateTime currentDate);

    public abstract MilestoneClaimBase WithCostsIncurred(bool? costsIncurred);

    public abstract MilestoneClaimBase WithConfirmation(bool? isConfirmed);

    public abstract MilestoneWithoutClaim Cancel();

    public abstract SubmittedMilestoneClaim Submit(DateTime currentDate);

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
