using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;
using PhoneNumbers;

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
                errorMessage ?? ValidationErrorMessage.StringLengthExceeded(_fieldLabel, MaximumInputLength.ShortInput));
            return this;
        }

        try
        {
            IsValidUkPhoneNumber(errorMessage);
        }
        catch (NumberParseException)
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterUkTelephoneNumber);
        }

        return this;
    }

    private void IsValidUkPhoneNumber(string? errorMessage)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var ukRegionCode = "GB";

        var phoneNumber = phoneNumberUtil.Parse(_telephoneNumber, TelephoneNumberRecognizer.StartWithCountryCode(_telephoneNumber!) ? null : ukRegionCode);

        if (!phoneNumberUtil.IsPossibleNumber(phoneNumber))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterUkTelephoneNumber);
        }

        if (!phoneNumberUtil.IsValidNumberForRegion(phoneNumber, ukRegionCode))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterTelephoneNumberInValidFormat);
        }

        if (phoneNumber.NationalNumber.ToString(CultureInfo.InvariantCulture) != TelephoneNumberRecognizer.StripToNationalFormat(_telephoneNumber!))
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterTelephoneNumberInValidFormat);
        }
    }
}
