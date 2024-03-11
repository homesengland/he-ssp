using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public class NumericValidator
{
    private readonly string? _value;
    private readonly decimal? _parsedDecimalValue;
    private readonly string _fieldName;
    private readonly string _fieldLabel;
    private readonly OperationResult _operationResult;

    private NumericValidator(string? value, string fieldName, string fieldLabel, OperationResult operationResult)
    {
        _value = value;
        _fieldName = fieldName;
        _fieldLabel = fieldLabel;
        _operationResult = operationResult;

        if (!string.IsNullOrWhiteSpace(_value) &&
            decimal.TryParse(
                _value.Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var parsedDecimalValue))
        {
            _parsedDecimalValue = parsedDecimalValue;
        }
    }

    public static implicit operator decimal?(NumericValidator v) => v._parsedDecimalValue;

    public static implicit operator long?(NumericValidator v) => ConvertToLong(v._parsedDecimalValue);

    public static implicit operator int?(NumericValidator v) => ConvertToInt(v._parsedDecimalValue);

    public static NumericValidator For(string? input, string fieldName, string fieldLabel, OperationResult? operationResult = null)
    {
        return new NumericValidator(input, fieldName, fieldLabel, operationResult ?? OperationResult.New());
    }

    public NumericValidator IsProvided(string? errorMessage = null)
    {
        if (string.IsNullOrEmpty(_value))
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MissingRequiredField(_fieldLabel));
        }

        return this;
    }

    public NumericValidator IsConditionallyRequired(bool condition, string? errorMessage = null)
    {
        if (condition && string.IsNullOrEmpty(_value))
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MissingRequiredField(_fieldLabel));
        }

        return this;
    }

    public NumericValidator IsNumber(string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_parsedDecimalValue == null)
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MustBeNumber(_fieldLabel));
        }

        return this;
    }

    public NumericValidator IsWholeNumber(string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_parsedDecimalValue % 1 != 0)
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MustBeWholeNumber(_fieldLabel));
        }

        return this;
    }

    public NumericValidator IsLessOrEqualTo(decimal maxValue, string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_parsedDecimalValue > maxValue)
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MustProvideTheLowerNumber(_fieldLabel, (int)maxValue));
        }

        return this;
    }

    public NumericValidator IsGreaterOrEqualTo(decimal minValue, string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_parsedDecimalValue < minValue)
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MustProvideTheHigherNumber(_fieldLabel, (int)minValue));
        }

        return this;
    }

    private static long? ConvertToLong(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        try
        {
            return Convert.ToInt64(Math.Floor(value.Value));
        }
        catch (OverflowException)
        {
            return long.MaxValue;
        }
    }

    private static int? ConvertToInt(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        try
        {
            return Convert.ToInt32(Math.Floor(value.Value));
        }
        catch (OverflowException)
        {
            return int.MaxValue;
        }
    }

    private void AddError(string fieldName, string? errorMessage = null)
    {
        if (!_operationResult.HasValidationErrors)
        {
            _operationResult.AddValidationError(fieldName, errorMessage ?? ValidationErrorMessage.InvalidValue);
        }
    }
}
