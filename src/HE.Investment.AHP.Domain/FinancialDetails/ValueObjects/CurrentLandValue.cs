using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class CurrentLandValue : PoundsValueObject
{
    public static readonly UiFields Fields = new(FinancialDetailsValidationFieldNames.LandValue, "current value of the land");

    public CurrentLandValue(decimal landValue)
        : base(landValue, Fields)
    {
    }

    public CurrentLandValue(string landValue)
        : base(landValue, Fields)
    {
    }
}
