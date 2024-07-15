using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class MilestoneClaim : ValueObject
{
    public MilestoneClaim(
        MilestoneType type,
        MilestoneStatus status,
        GrantApportioned grantApportioned,
        ClaimDate claimDate,
        bool? costIncurred,
        bool? isConfirmed)
    {
        Type = type;
        Status = status;
        GrantApportioned = grantApportioned;
        ClaimDate = claimDate;
        CostIncurred = costIncurred;
        IsConfirmed = isConfirmed;
    }

    public MilestoneType Type { get; }

    public MilestoneStatus Status { get; }

    public GrantApportioned GrantApportioned { get; }

    public ClaimDate ClaimDate { get; }

    public bool? CostIncurred { get; }

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

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return Status;
        yield return GrantApportioned;
        yield return ClaimDate;
        yield return CostIncurred;
        yield return IsConfirmed;
    }
}
