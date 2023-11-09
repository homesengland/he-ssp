using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation;

public class NumericValidator
{
    private readonly decimal? _value;
    private readonly string _fieldName;
    private readonly OperationResult _operationResult;

    private NumericValidator(decimal? value, string fieldName, OperationResult operationResult)
    {
        _value = value;
        _fieldName = fieldName;
        _operationResult = operationResult;
    }

    public static implicit operator decimal(NumericValidator v) => v._value ?? 0;

    public static implicit operator int(NumericValidator v) => Convert.ToInt32(v._value ?? 0);

    public static NumericValidator For(decimal? input, string fieldName, OperationResult? operationResult = null)
    {
        return new NumericValidator(input, fieldName, operationResult ?? OperationResult.New());
    }

    public NumericValidator IsProvided(string? errorMessage = null)
    {
        if (_value == null)
        {
            AddError(_fieldName, errorMessage ?? $"Value for {_fieldName} is not provided.");
        }

        return this;
    }

    public NumericValidator IsNotDefault(string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_value == 0)
        {
            AddError(_fieldName, errorMessage ?? $"Value for {_fieldName} should not be zero.");
        }

        return this;
    }

    public NumericValidator IsPositive(string? errorMessage = null)
    {
        if (_value == null)
        {
            return this;
        }

        if (_value <= 0)
        {
            AddError(_fieldName, errorMessage ?? $"Value for {_fieldName} should be positive.");
        }

        return this;
    }

    private void AddError(string fieldName, string? errorMessage = null)
    {
        _operationResult.AddValidationError(fieldName, errorMessage ?? ValidationErrorMessage.InvalidValue);
    }
}
