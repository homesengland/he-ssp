using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public static class WholeNumberRangeValidator
{
    public static decimal ValidateDeffer(decimal number, string fieldName, string displayName, OperationResult operationResult)
    {
        number = Math.Round(number, 0);
        if (number is < 0 or > 999999999)
        {
            operationResult.AddValidationError(fieldName, ValidationErrorMessage.MustBeWholeNumberBetween(displayName, 0, 999999999));
        }

        return number;
    }

    public static decimal Validate(decimal number, string fieldName, string displayName)
    {
        var operationResult = OperationResult.New();
        var value = ValidateDeffer(number, fieldName, displayName, operationResult);

        operationResult.CheckErrors();
        return value;
    }
}
