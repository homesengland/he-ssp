using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public static class PoundsPencesValidator
{
    public static decimal ValidateDeffer(decimal pounds, string fieldName, string displayName, decimal? maxValue, string? maxValueExceededError, OperationResult operationResult)
    {
        pounds = Math.Round(pounds, 2);
        if (pounds < 0)
        {
            operationResult.AddValidationError(fieldName, ValidationErrorMessage.PoundInput(displayName));
        }

        if (maxValue != null && pounds > maxValue)
        {
            operationResult.AddValidationError(fieldName, maxValueExceededError ?? ValidationErrorMessage.PoundInput(displayName));
        }

        return pounds;
    }

    public static decimal Validate(decimal? pounds, string fieldName, string displayName, decimal? maxValue = null, string? maxValueExceededError = null)
    {
        if (pounds is null)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.PoundInput(displayName));
        }

        var operationResult = OperationResult.New();
        var value = ValidateDeffer(pounds!.Value, fieldName, displayName, maxValue, maxValueExceededError, operationResult);

        operationResult.CheckErrors();
        return value;
    }
}
