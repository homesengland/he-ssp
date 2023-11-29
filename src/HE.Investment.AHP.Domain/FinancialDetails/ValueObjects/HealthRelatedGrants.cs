using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class HealthRelatedGrants : NumericValueObject
{
    public HealthRelatedGrants(string value)
        : base(FinancialDetailsValidationFieldNames.HeatlthRelatedGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static HealthRelatedGrants? From(decimal? value)
    {
        return value.HasValue ? new HealthRelatedGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
