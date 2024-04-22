using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class CurrentLandValue : TheRequiredIntValueObject
{
    public CurrentLandValue(string landValue)
    : base(landValue, FinancialDetailsValidationFieldNames.LandValue, "current value of the land", 0, 999999999, MessageOptions.Money)
    {
    }

    private CurrentLandValue(int landValue)
        : base(landValue, FinancialDetailsValidationFieldNames.LandValue, "current value of the land")
    {
    }

    public static CurrentLandValue FromCrm(decimal value) => new(decimal.ToInt32(value));
}
