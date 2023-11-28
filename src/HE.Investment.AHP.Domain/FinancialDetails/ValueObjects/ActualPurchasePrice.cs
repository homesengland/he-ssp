using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ActualPurchasePrice : NumericValueObject
{
    public ActualPurchasePrice(string value)
        : base(FinancialDetailsValidationFieldNames.ActualPurchasePrice, value, FinancialDetailsValidationErrors.InvalidActualPurchasePrice)
    {
    }

    public static ActualPurchasePrice? From(decimal? value)
    {
        return value.HasValue ? new ActualPurchasePrice(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
