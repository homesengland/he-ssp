using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;

namespace HE.Investments.Common.Validators;

public class TelephoneNumberValidator
{
    private readonly string? _telephoneNumber;
    private readonly string _fieldName;
    private readonly string _fieldLabel;
    private readonly OperationResult _operationResult;

    private TelephoneNumberValidator(string? telephoneNumber, string fieldName, string fieldLabel, OperationResult operationResult)
    {
        _telephoneNumber = telephoneNumber;
        _fieldName = fieldName;
        _fieldLabel = fieldLabel;
        _operationResult = operationResult;
    }

    public static implicit operator string?(TelephoneNumberValidator v) => v._telephoneNumber ?? null;

    public static TelephoneNumberValidator For(
        string? telephoneNumber,
        string fieldName,
        string fieldLabel,
        OperationResult? operationResult = null)
    {
        return new TelephoneNumberValidator(telephoneNumber, fieldName, fieldLabel, operationResult ?? OperationResult.New());
    }

    public TelephoneNumberValidator IsValid(string? errorMessage = null)
    {
        if (string.IsNullOrWhiteSpace(_telephoneNumber))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.MissingRequiredField(_fieldLabel));
            return this;
        }

        if (_telephoneNumber!.Length > MaximumInputLength.ShortInput)
        {
            _operationResult.AddValidationError(
                _fieldName,
                errorMessage ?? ValidationErrorMessage.StringLengthExceededUncommon(_fieldLabel, MaximumInputLength.ShortInput));
            return this;
        }

        ValidateUkPhoneNumber(errorMessage);

        return this;
    }

    private void ValidateUkPhoneNumber(string? errorMessage)
    {
        const int ukTelephoneNumberLengthWithoutPrefix = 10;

        if (_telephoneNumber!.Contains("--"))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterTelephoneNumberInValidFormat);
            return;
        }

        var telephoneNumber = TelephoneNumberRecognizer.StripFromSpecialCharacters(_telephoneNumber!);

        if (TelephoneNumberRecognizer.StartWithCountryCode(telephoneNumber) && !TelephoneNumberRecognizer.IsUkCountryCode(telephoneNumber))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterUkTelephoneNumber);
            return;
        }

        telephoneNumber = TelephoneNumberRecognizer.StripToNationalFormat(telephoneNumber);

        if (telephoneNumber.Length != ukTelephoneNumberLengthWithoutPrefix || !telephoneNumber.All(char.IsDigit) || telephoneNumber.StartsWith('0'))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterTelephoneNumberInValidFormat);
        }
    }
}
