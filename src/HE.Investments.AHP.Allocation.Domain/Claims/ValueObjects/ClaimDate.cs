using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class ClaimDate : ValueObject
{
    public ClaimDate(DateTime forecastClaimDate, AchievementDate? achievementDate = null, DateTime? submissionDate = null)
    {
        ForecastClaimDate = forecastClaimDate;
        AchievementDate = achievementDate;
        SubmissionDate = submissionDate;
    }

    public DateTime ForecastClaimDate { get; }

    public AchievementDate? AchievementDate { get; }

    public DateTime? SubmissionDate { get; }

    public bool IsAnswered()
    {
        return ForecastClaimDate.IsProvided() && AchievementDate.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ForecastClaimDate;
        yield return AchievementDate;
        yield return SubmissionDate;
    }
}
