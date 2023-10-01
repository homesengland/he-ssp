using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class Coordinates
{
    public Coordinates(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(Coordinates), ValidationErrorMessage.EnterCoordinates)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(nameof(Coordinates), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationCoordinates))
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; private set; }
}
