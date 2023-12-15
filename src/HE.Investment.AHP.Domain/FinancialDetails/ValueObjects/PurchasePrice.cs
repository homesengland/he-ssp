using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : PoundsPenceValueObject
{
    public static readonly UiFields Fields = new(FinancialDetailsValidationFieldNames.PurchasePrice, "The purchase price of the land");

    public PurchasePrice(decimal landValue)
        : base(landValue)
    {
    }

    public PurchasePrice(string landValue)
        : base(landValue, FinancialDetailsValidationErrors.InvalidActualPurchasePrice)
    {
    }

    public override UiFields UiFields => Fields;
}
