using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

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
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    affectedField,
                    ValidationErrorMessage
                        .ShortInputLengthExceeded(FieldNameForInputLengthValidation.TelephoneNumberType(affectedField)))
                .CheckErrors();
        }

        Value = value;
        AffectedField = affectedField;
    }

    public string Value { get; }

    public string AffectedField { get; }

    public static TelephoneNumber New(string value, string affectedField = nameof(UserDetailsViewModel.TelephoneNumber)) => new(value, affectedField);

    public static TelephoneNumber? FromString(string? value, string affectedField = nameof(UserDetailsViewModel.TelephoneNumber))
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
