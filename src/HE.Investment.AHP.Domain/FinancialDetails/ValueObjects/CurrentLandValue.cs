using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class CurrentLandValue : TheRequiredIntValueObject
{
    public CurrentLandValue(string landValue)
    : base(landValue, FinancialDetailsValidationFieldNames.LandValue, "current value of the land", 0, 999999999)
    {
    }

    private CurrentLandValue(int landValue)
        : base(landValue, FinancialDetailsValidationFieldNames.LandValue, "current value of the land")
    {
    }

    public static CurrentLandValue FromCrm(decimal value) => new(decimal.ToInt32(value));
}
