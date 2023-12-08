using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public static class WholeNumberValidator
{
    public static decimal ValidateDeffer(decimal pounds, string fieldName, string displayName, OperationResult operationResult, string? validationMessage = null)
    {
        pounds = Math.Round(pounds, 0);
        if (pounds is < 0 or > 999999999)
        {
            operationResult.AddValidationError(fieldName, validationMessage ?? ValidationErrorMessage.WholeNumberInput(displayName));
        }

        return pounds;
    }

    public static decimal Validate(decimal pounds, string fieldName, string displayName, string? validationMessage = null)
    {
        var operationResult = OperationResult.New();
        var value = ValidateDeffer(pounds, fieldName, displayName, operationResult, validationMessage);

        operationResult.CheckErrors();
        return value;
    }
}
