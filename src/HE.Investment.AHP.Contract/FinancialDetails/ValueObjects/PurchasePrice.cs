using Dawn;
using HE.Investment.AHP.Contract.Domain;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;

namespace HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
public class PurchasePrice : ValueObject
{
    public PurchasePrice(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.NoPurchasePrice)
                .CheckErrors();
        }

        if (!int.TryParse(value, out var price) || price <= 0)
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidPurchasePrice)
                .CheckErrors();
        }

        Value = price;
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
