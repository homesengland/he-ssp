using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : PoundsValueObject
{
    public static readonly UiFields Fields = new(FinancialDetailsValidationFieldNames.PurchasePrice, "purchase price of the land");

    public PurchasePrice(decimal landValue)
        : base(landValue, Fields)
    {
    }

    public PurchasePrice(string landValue)
        : base(landValue, Fields)
    {
    }
}
