using System.Globalization;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Organization;

public class NumericValidator
{
    private readonly string? _value;
    private readonly long? _parsedValue;
    private readonly string _fieldName;
    private readonly OperationResult _operationResult;

    private NumericValidator(string? value, string fieldName, OperationResult operationResult)
    {
        _value = value;
        _fieldName = fieldName;
        _operationResult = operationResult;
        if (long.TryParse(_value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            _parsedValue = parsedValue;
        }
    }

    public static implicit operator long(NumericValidator v) => v._parsedValue ?? 0;

    public static implicit operator int(NumericValidator v) => Convert.ToInt32(v._parsedValue ?? 0);

    public static NumericValidator For(string? input, string fieldName, OperationResult? operationResult = null)
    {
        return new NumericValidator(input, fieldName, operationResult ?? OperationResult.New());
    }

    public NumericValidator IsProvided(string? errorMessage = null)
    {
        if (string.IsNullOrEmpty(_value))
        {
            AddError(_fieldName, errorMessage ?? $"Value for {_fieldName} is not provided.");
        }

        return this;
    }

    public NumericValidator IsWholeNumber(string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (!long.TryParse(_value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var _))
        {
            AddError(_fieldName, errorMessage ?? $"Value for {_fieldName} should be a number.");
        }

        return this;
    }

    public NumericValidator IsBetween(long minValue = 1, long maxValue = 99999999999, string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_parsedValue < minValue || _parsedValue > maxValue)
        {
            AddError(_fieldName, errorMessage ?? $"Value for {_fieldName} should be between {minValue} and {maxValue}.");
        }

        return this;
    }

    private void AddError(string fieldName, string? errorMessage = null)
    {
        _operationResult.AddValidationError(fieldName, errorMessage ?? ValidationErrorMessage.InvalidValue);
    }
}
