using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

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
