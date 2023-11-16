using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : ValueObject
{
    public PurchasePrice(string value, bool isPurchasePriceKnown)
    {
        int intValue;
        if (isPurchasePriceKnown)
        {
            var operationResult = OperationResult.New();

            intValue = NumericValidator
                .For(value, FinancialDetailsValidationFieldNames.PurchasePrice, operationResult)
                .IsWholeNumber(FinancialDetailsValidationErrors.InvalidPurchasePrice)
                .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidPurchasePrice);

            operationResult.CheckErrors();

            Value = intValue;
        }
        else
        {
            var operationResult = OperationResult.New();

            intValue = NumericValidator
                .For(value, FinancialDetailsValidationFieldNames.PurchasePrice, operationResult)
                .IsWholeNumber(FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
                .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice);

            operationResult.CheckErrors();

            Value = intValue;
        }
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
