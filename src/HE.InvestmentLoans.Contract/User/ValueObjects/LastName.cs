using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class LastName : ValueObjectWithErrorItem
{
    private LastName(string value)
    {
        if (value!.IsNotProvided())
        {
            AddValidationError(nameof(UserDetailsViewModel.LastName), ValidationErrorMessage.EnterLastName);
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            AddValidationError(
                nameof(UserDetailsViewModel.LastName),
                ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.LastName));
        }

        Value = value;
    }

    private string Value { get; }

    public static LastName New(string value) => new(value);

    public static LastName FromString(string lastName) => new(lastName);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
