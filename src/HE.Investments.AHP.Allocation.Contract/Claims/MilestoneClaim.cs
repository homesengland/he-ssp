using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Allocation.Contract.Claims;

public record MilestoneClaim(
    MilestoneType Type,
    MilestoneStatus Status,
    decimal AmountOfGrantApportioned,
    decimal PercentageOfGrantApportioned,
    DateDetails ForecastClaimDate,
    DateDetails? AchievementDate,
    DateDetails? SubmissionDate,
    bool CanBeClaimed,
    bool? CostsIncurred,
    bool? IsConfirmed);
