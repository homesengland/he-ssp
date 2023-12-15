using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedPurchasePrice : PoundsPenceValueObject
{
    public static readonly UiFields Fields = new(FinancialDetailsValidationFieldNames.PurchasePrice, "The expected purchase price of the land");

    public ExpectedPurchasePrice(decimal landValue)
        : base(landValue)
    {
    }

    public ExpectedPurchasePrice(string landValue)
        : base(landValue, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
    {
    }

    public override UiFields UiFields => Fields;
}
