using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class CurrentLandValue : PoundsPenceValueObject
{
    public static readonly UiFields Fields = new(FinancialDetailsValidationFieldNames.LandValue, "Current Land Value");

    public CurrentLandValue(decimal landValue)
        : base(landValue)
    {
    }

    public CurrentLandValue(string landValue)
        : base(landValue, FinancialDetailsValidationErrors.InvalidLandValue)
    {
    }

    public override UiFields UiFields => Fields;
}
