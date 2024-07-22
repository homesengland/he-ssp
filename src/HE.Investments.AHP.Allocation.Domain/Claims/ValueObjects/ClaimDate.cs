using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;

public class ClaimDate : ValueObject
{
    public ClaimDate(DateTime forecastClaimDate, DateTime? actualClaimDate = null)
    {
        ForecastClaimDate = forecastClaimDate;
        ActualClaimDate = actualClaimDate;
    }

    public DateTime ForecastClaimDate { get; }

    public DateTime? ActualClaimDate { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ForecastClaimDate;
        yield return ActualClaimDate;
    }
}
