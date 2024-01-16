using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public static class PoundsPencesValidator
{
    public static decimal ValidateDeffer(decimal pounds, string fieldName, string displayName, decimal? maxValue, OperationResult operationResult)
    {
        pounds = Math.Round(pounds, 2);
        if (pounds < 0)
        {
            operationResult.AddValidationError(fieldName, ValidationErrorMessage.PoundInput(displayName));
        }

        if (maxValue != null && pounds > maxValue)
        {
            operationResult.AddValidationError(fieldName, ValidationErrorMessage.PoundInput(displayName));
        }

        return pounds;
    }

    public static decimal Validate(decimal pounds, string fieldName, string displayName, decimal? maxValue = null)
    {
        var operationResult = OperationResult.New();
        var value = ValidateDeffer(pounds, fieldName, displayName, maxValue, operationResult);

        operationResult.CheckErrors();
        return value;
    }
}
