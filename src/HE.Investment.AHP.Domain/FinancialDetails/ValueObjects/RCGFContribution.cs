using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class RCGFContribution : NumericValueObject
{
    public RCGFContribution(string value)
        : base(FinancialDetailsValidationFieldNames.RCGFContribution, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static RCGFContribution? From(decimal? value)
    {
        return value.HasValue ? new RCGFContribution(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
