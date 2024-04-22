using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedPurchasePrice : TheRequiredIntValueObject
{
    public ExpectedPurchasePrice(string landValue)
        : base(landValue, FinancialDetailsValidationFieldNames.PurchasePrice, "expected purchase price of the land", 0, 999999999, MessageOptions.IsMoney)
    {
    }

    private ExpectedPurchasePrice(int landValue)
    : base(landValue, FinancialDetailsValidationFieldNames.PurchasePrice, "expected purchase price of the land")
    {
    }

    public static ExpectedPurchasePrice FromCrm(decimal value) => new(decimal.ToInt32(value));
}
