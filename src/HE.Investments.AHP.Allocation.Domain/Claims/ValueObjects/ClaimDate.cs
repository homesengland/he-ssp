using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class ClaimDate : ValueObject
{
    public ClaimDate(DateTime forecastClaimDate, AchievementDate? achievementDate = null, DateDetails? submissionDate = null)
    {
        ForecastClaimDate = forecastClaimDate;
        AchievementDate = achievementDate;
        SubmissionDate = submissionDate;
    }

    public DateTime ForecastClaimDate { get; }

    public AchievementDate? AchievementDate { get; private set; }

    public DateDetails? SubmissionDate { get; }

    public void WithAchievementDate(AchievementDate? achievementDate) => AchievementDate = achievementDate;

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ForecastClaimDate;
        yield return AchievementDate;
        yield return SubmissionDate;
    }
}
