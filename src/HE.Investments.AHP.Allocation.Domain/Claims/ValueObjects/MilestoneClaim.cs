using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.Common.Domain;

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
