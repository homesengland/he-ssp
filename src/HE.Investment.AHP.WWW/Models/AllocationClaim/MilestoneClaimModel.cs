using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Models.AllocationClaim;

public class MilestoneClaimModel(
    MilestoneType type,
    MilestoneStatus status,
    decimal amountOfGrantApportioned,
    decimal percentageOfGrantApportioned,
    DateDetails forecastClaimDate,
    DateDetails? achievementDate,
    DateDetails? submissionDate,
    bool canBeClaimed,
    bool? costsIncurred,
    bool? isConfirmed)
{
    public MilestoneType Type { get; set; } = type;

    public MilestoneStatus Status { get; set; } = status;

    public decimal AmountOfGrantApportioned { get; set; } = amountOfGrantApportioned;

    public decimal PercentageOfGrantApportioned { get; set; } = percentageOfGrantApportioned;

    public DateDetails ForecastClaimDate { get; set; } = forecastClaimDate;

    public DateDetails? AchievementDate { get; set; } = achievementDate;

    public DateDetails? SubmissionDate { get; set; } = submissionDate;

    public bool CanBeClaimed { get; set; } = canBeClaimed;

    public bool? CostsIncurred { get; set; } = costsIncurred;

    public bool? IsConfirmed { get; set; } = isConfirmed;
}
