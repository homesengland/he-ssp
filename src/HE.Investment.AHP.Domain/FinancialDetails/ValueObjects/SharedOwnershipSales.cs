using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class SharedOwnershipSales : NumericValueObject
{
    public SharedOwnershipSales(string value)
        : base(FinancialDetailsValidationFieldNames.SharedOwnershipSales, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static SharedOwnershipSales? From(decimal? value)
    {
        return value.HasValue ? new SharedOwnershipSales(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
