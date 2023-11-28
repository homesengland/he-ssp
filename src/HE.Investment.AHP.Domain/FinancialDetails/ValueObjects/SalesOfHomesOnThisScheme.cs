using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class SalesOfHomesOnThisScheme : NumericValueObject
{
    public SalesOfHomesOnThisScheme(string value)
        : base(FinancialDetailsValidationFieldNames.SaleOfHomesOnThisScheme, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static SalesOfHomesOnThisScheme? From(decimal? value)
    {
        return value.HasValue ? new SalesOfHomesOnThisScheme(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
