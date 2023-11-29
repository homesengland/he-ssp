using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LandValue : NumericValueObject
{
    public LandValue(string value)
        : base(FinancialDetailsValidationFieldNames.LandValue, value, FinancialDetailsValidationErrors.InvalidLandValue)
    {
    }

    public static LandValue? From(decimal? value)
    {
        return value.HasValue ? new LandValue(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
