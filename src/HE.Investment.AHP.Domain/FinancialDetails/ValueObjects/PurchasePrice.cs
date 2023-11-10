using Dawn;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : ValueObject
{
    public PurchasePrice(string value, bool isPurchasePriceKnown)
    {
        if (!int.TryParse(value, out var price))
        {
            if (isPurchasePriceKnown)
            {
                OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidPurchasePrice)
                .CheckErrors();
            }
            else
            {
                OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
                .CheckErrors();
            }
        }

        Value = price;
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
