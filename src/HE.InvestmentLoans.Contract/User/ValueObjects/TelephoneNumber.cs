using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class TelephoneNumber : ValueObject
{
    public TelephoneNumber(string value, string affectedField)
    {
        if (value!.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(affectedField, ValidationErrorMessage.EnterTelephoneNumber)
                .CheckErrors();
        }
        else if (affectedField == nameof(UserDetailsViewModel.TelephoneNumber) && value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    affectedField,
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.TelephoneNumber))
                .CheckErrors();
        }
        else if (affectedField == nameof(UserDetailsViewModel.SecondaryTelephoneNumber) && value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    affectedField,
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.SecondaryTelephoneNumber))
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    public static TelephoneNumber New(string value, string affectedField) => new(value, affectedField);

    public static TelephoneNumber? FromString(string? value, string affectedField)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return new TelephoneNumber(value, affectedField);
    }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
