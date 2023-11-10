using System.Text.RegularExpressions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Domain.Organisation;

public class Validator
{
    private readonly string? _value;
    private readonly string _fieldName;
    private readonly OperationResult _operationResult;
    private bool _isError;

    private Validator(string? value, string fieldName, OperationResult operationResult)
    {
        _value = value;
        _fieldName = fieldName;
        _operationResult = operationResult;
    }

    public static implicit operator string(Validator v) => v._value!;

    public static Validator For(string? input, string fieldName, OperationResult? operationResult = null)
    {
        return new Validator(input, fieldName, operationResult ?? OperationResult.New());
    }

    public Validator IsProvided(string? errorMessage = null)
    {
        if (!_value.IsProvided())
        {
            AddError(_fieldName, errorMessage);
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
            AddError(_fieldName, errorMessage ?? GenericValidationError.TextTooLong);
        }

        return this;
    }

    public Validator IsLongInput()
    {
        if (_value.IsNotProvided())
        {
            return this;
        }

        if (_value!.Length > MaximumInputLength.LongInput)
        {
            AddError(_fieldName, GenericValidationError.TextTooLong);
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
