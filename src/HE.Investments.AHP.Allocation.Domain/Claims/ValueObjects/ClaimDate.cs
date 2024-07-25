using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class ClaimDate : ValueObject
{
    public ClaimDate(DateTime forecastClaimDate, DateDetails? achievementDate = null, DateDetails? submissionDate = null)
    {
        ForecastClaimDate = forecastClaimDate;
        AchievementDate = achievementDate;
        SubmissionDate = submissionDate;
    }

    public DateTime ForecastClaimDate { get; }

    public DateDetails? AchievementDate { get; }

    public DateDetails? SubmissionDate { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ForecastClaimDate;
        yield return AchievementDate;
        yield return SubmissionDate;
    }
}
