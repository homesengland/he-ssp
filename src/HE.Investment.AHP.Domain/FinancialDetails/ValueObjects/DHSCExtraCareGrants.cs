using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class DHSCExtraCareGrants : NumericValueObject
{
    public DHSCExtraCareGrants(string value)
        : base(FinancialDetailsValidationFieldNames.DHSCExtraCareGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static DHSCExtraCareGrants? From(decimal? value)
    {
        return value.HasValue ? new DHSCExtraCareGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
