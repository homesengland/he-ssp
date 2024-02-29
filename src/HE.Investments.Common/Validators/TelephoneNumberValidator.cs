using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using PhoneNumbers;

namespace HE.Investments.Common.Validators;

public class TelephoneNumberValidator
{
    private readonly string? _telephoneNumber;
    private readonly string _fieldName;
    private readonly string _fieldLabel;
    private readonly bool _isOptional;
    private readonly OperationResult _operationResult;

    private TelephoneNumberValidator(string? telephoneNumber, string fieldName, string fieldLabel, bool isOptional, OperationResult operationResult)
    {
        _telephoneNumber = telephoneNumber;
        _fieldName = fieldName;
        _fieldLabel = fieldLabel;
        _isOptional = isOptional;
        _operationResult = operationResult;
    }

    public static implicit operator string?(TelephoneNumberValidator v) => v._telephoneNumber ?? null;

    public static TelephoneNumberValidator For(
        string? telephoneNumber,
        string fieldName,
        string fieldLabel,
        bool isOptional,
        OperationResult? operationResult = null)
    {
        return new TelephoneNumberValidator(telephoneNumber, fieldName, fieldLabel, isOptional, operationResult ?? OperationResult.New());
    }

    public TelephoneNumberValidator IsValid(string? errorMessage = null)
    {
        if (string.IsNullOrWhiteSpace(_telephoneNumber) && !_isOptional)
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
            IsPossiblePhoneNumber();
        }
        catch (NumberParseException)
        {
            _operationResult.AddValidationError(_fieldName, errorMessage ?? ValidationErrorMessage.EnterUkTelephoneNumber);
        }

        return this;
    }

    private void IsPossiblePhoneNumber()
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        PhoneNumber phoneNumber;

        if (_telephoneNumber.IsProvided() && _telephoneNumber!.StartsWith("+", StringComparison.InvariantCulture))
        {
            phoneNumber = phoneNumberUtil.Parse(_telephoneNumber, null);
        }
        else
        {
            phoneNumber = phoneNumberUtil.Parse(_telephoneNumber, "GB");
        }

        if (!phoneNumberUtil.IsPossibleNumber(phoneNumber))
        {
            throw new NumberParseException(ErrorType.NOT_A_NUMBER, $"{phoneNumber} is not a valid phone number");
        }
    }
}
