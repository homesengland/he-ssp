using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class FirstName : ValueObjectWithErrorItem
{
    private FirstName(string value)
    {
        if (value!.IsNotProvided())
        {
            AddValidationError(nameof(UserDetailsViewModel.FirstName), ValidationErrorMessage.EnterFirstName);
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            AddValidationError(
                nameof(UserDetailsViewModel.FirstName),
                ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.FirstName));
        }

        Value = value;
    }

    private string Value { get; }

    public static FirstName New(string value) => new(value);

    public static FirstName FromString(string firstName) => new(firstName);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
