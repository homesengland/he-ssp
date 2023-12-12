using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class LandValue : ValueObject, IQuestion
{
    public LandValue(CurrentLandValue? currentLandValue, bool? isPublicLand)
    {
        CurrentLandValue = currentLandValue;
        IsPublicLand = isPublicLand;
    }

    public CurrentLandValue? CurrentLandValue { get; }

    public bool? IsPublicLand { get; }

    public bool IsAnswered()
    {
        return CurrentLandValue.IsProvided() && IsPublicLand.IsProvided();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CurrentLandValue;
        yield return IsPublicLand;
    }
}
