using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public static class PoundsValidator
{
    public static decimal ValidateDeffer(decimal pounds, string fieldName, string displayName, OperationResult operationResult)
    {
        pounds = Math.Round(pounds, 2);
        if (pounds < 0)
        {
            operationResult.AddValidationError(fieldName, ValidationErrorMessage.PoundInput(displayName));
        }

        return pounds;
    }

    public static decimal Validate(decimal pounds, string fieldName, string displayName)
    {
        var operationResult = OperationResult.New();
        var value = ValidateDeffer(pounds, fieldName, displayName, operationResult);

        operationResult.CheckErrors();
        return value;
    }
}
