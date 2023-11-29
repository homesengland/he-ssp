using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedPurchasePrice : NumericValueObject
{
    public ExpectedPurchasePrice(string value)
        : base(FinancialDetailsValidationFieldNames.ExpectedPurchasePrice, value, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
    {
    }

    public static ExpectedPurchasePrice? From(decimal? value)
    {
        return value.HasValue ? new ExpectedPurchasePrice(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
