using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class SalesOfHomesOnOtherSchemes : NumericValueObject
{
    public SalesOfHomesOnOtherSchemes(string value)
        : base(FinancialDetailsValidationFieldNames.SaleOfHomesOnOtherSchemes, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static SalesOfHomesOnOtherSchemes? From(decimal? value)
    {
        return value.HasValue ? new SalesOfHomesOnOtherSchemes(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
