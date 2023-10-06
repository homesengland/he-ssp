using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class LastName : ValueObject
{
    public LastName(string value)
    {
        if (value!.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(UserDetailsViewModel.LastName), ValidationErrorMessage.EnterLastName)
                .CheckErrors();
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    nameof(UserDetailsViewModel.LastName),
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.LastName))
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    public static LastName New(string value) => new(value);

    public static LastName? FromString(string? lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            return null;
        }

        return new LastName(lastName);
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
