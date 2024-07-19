using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class MilestoneClaim : ValueObject
{
    public MilestoneClaim(
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
                nameof(CostsIncurred), "Costs incurred can be provided only for Acquisition milestone");
        }

        Type = type;
        Status = status;
        GrantApportioned = grantApportioned;
        ClaimDate = claimDate;
        CostsIncurred = costsIncurred;
        IsConfirmed = isConfirmed;
    }

    public MilestoneType Type { get; }

    public MilestoneStatus Status { get; }

    public GrantApportioned GrantApportioned { get; }

    public ClaimDate ClaimDate { get; }

    public bool? CostsIncurred { get; }

    public bool? IsConfirmed { get; }

    public bool IsSubmitted => Status >= MilestoneStatus.Submitted;

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

    public MilestoneClaim WithCostsIncurred(bool? costsIncurred)
    {
        if (costsIncurred.IsNotProvided())
        {
            OperationResult.ThrowValidationError(
                nameof(CostsIncurred),
                ValidationErrorMessage.MustBeSelectedYes(
                    "you have incurred costs and made payments to at least the level of the grant"));
        }

        return new MilestoneClaim(
            Type,
            Status,
            GrantApportioned,
            ClaimDate,
            costsIncurred,
            IsConfirmed);
    }

    public MilestoneClaim WithConfirmation(bool? isConfirmed)
    {
        if (isConfirmed != true)
        {
            OperationResult.ThrowValidationError(nameof(IsConfirmed), "Confirm the declaration to continue");
        }

        return new MilestoneClaim(
            Type,
            Status,
            GrantApportioned,
            ClaimDate,
            CostsIncurred,
            isConfirmed);
    }

    public CanceledMilestoneClaim Cancel()
    {
        if (Status != MilestoneStatus.Draft)
        {
            OperationResult.ThrowValidationError(nameof(MilestoneClaim), "Cannot cancel submitted claim");
        }

        return new CanceledMilestoneClaim(this);
    }

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
