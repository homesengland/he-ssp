using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class LandValue : ValueObject, IQuestion
{
    public LandValue(CurrentLandValue? currentLandValue, YesNoType isPublicLand)
    {
        CurrentLandValue = currentLandValue;
        IsPublicLand = isPublicLand;
    }

    public LandValue()
    {
    }

    public CurrentLandValue? CurrentLandValue { get; }

    public YesNoType IsPublicLand { get; }

    public bool IsAnswered()
    {
        return CurrentLandValue.IsProvided() && IsPublicLand != YesNoType.Undefined;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CurrentLandValue;
        yield return IsPublicLand;
    }
}
