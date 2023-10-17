using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class TelephoneNumber : ValueObject
{
    public TelephoneNumber(string value)
    {
        Value = value;
    }

    public TelephoneNumber(string value, string affectedField, string validationMessage)
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
                    validationMessage)
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    public static TelephoneNumber New(string value, string affectedField, string validationMessage) => new(value, affectedField, validationMessage);

    public static TelephoneNumber? FromString(string? telephoneNumber)
    {
        if (string.IsNullOrEmpty(telephoneNumber))
        {
            return null;
        }

        return new TelephoneNumber(telephoneNumber);
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
