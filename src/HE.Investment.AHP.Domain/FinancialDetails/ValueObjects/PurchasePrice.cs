using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : TheRequiredIntValueObject
{
    public PurchasePrice(string value)
        : base(value, FinancialDetailsValidationFieldNames.PurchasePrice, "purchase price of the land", 0, 999999999)
    {
    }

    private PurchasePrice(int value)
        : base(value, FinancialDetailsValidationFieldNames.PurchasePrice, "purchase price of the land")
    {
    }

    public static PurchasePrice FromCrm(decimal value) => new(decimal.ToInt32(value));
}
