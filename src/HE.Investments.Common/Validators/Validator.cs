using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Validators;

public class Validator
{
    private readonly string? _value;
    private readonly string _fieldName;
    private readonly string _fieldLabel;
    private readonly OperationResult _operationResult;
    private bool _isError;

    private Validator(string? value, string fieldName, string fieldLabel, OperationResult operationResult)
    {
        _value = value;
        _fieldName = fieldName;
        _fieldLabel = fieldLabel;
        _operationResult = operationResult;
    }

    public static implicit operator string(Validator v) => v._value!;

    public static Validator For(string? input, string fieldName, string fieldLabel, OperationResult? operationResult = null)
    {
        return new Validator(input, fieldName, fieldLabel, operationResult ?? OperationResult.New());
    }

    public Validator IsProvided(string? errorMessage = null)
    {
        if (!_value.IsProvided())
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MissingRequiredField(_fieldLabel));
        }

        return this;
    }

    public Validator IsProvidedIf(bool condition, string? errorMessage = null)
    {
        if (condition && !_value.IsProvided())
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.MissingRequiredField(_fieldLabel));
        }

        return this;
    }

    public Validator IsShortInput(string? errorMessage = null)
    {
        if (_value.IsNotProvided())
        {
            return this;
        }

        if (_value!.Length > MaximumInputLength.ShortInput)
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.ShortInputLengthExceeded(_fieldLabel));
        }

        return this;
    }

    public Validator IsLongInput(string? errorMessage = null)
    {
        if (_value.IsNotProvided())
        {
            return this;
        }

        if (_value!.Length > MaximumInputLength.LongInput)
        {
            AddError(_fieldName, errorMessage ?? ValidationErrorMessage.LongInputLengthExceeded(_fieldLabel));
        }

        return this;
    }

    public Validator IsValidPostcode(string? errorMessage = null)
    {
        if (_value.IsNotProvided())
        {
            return this;
        }

        MatchRegex("^[A-Z]{1,2}\\d[A-Z\\d]? ?\\d[A-Z]{2}$", errorMessage);

        return this;
    }

    private void MatchRegex(string regex, string? errorMessage = null)
    {
        if (_value.IsNotProvided())
        {
            return;
        }

        var match = Regex.Match(_value!, regex, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            AddError(_fieldName, errorMessage);
        }
    }

    private void AddError(string fieldName, string? errorMessage = null)
    {
        if (!_isError)
        {
            _operationResult.AddValidationError(fieldName, errorMessage ?? ValidationErrorMessage.InvalidValue);
            _isError = true;
        }
    }
}
