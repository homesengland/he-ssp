using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class TelephoneNumber : ValueObjectWithErrorItem
{
    private TelephoneNumber(string value)
    {
        if (value!.IsNotProvided())
        {
            AddValidationError(nameof(UserDetailsViewModel.TelephoneNumber), ValidationErrorMessage.EnterTelephoneNumber);
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            AddValidationError(
                nameof(UserDetailsViewModel.TelephoneNumber),
                ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.TelephoneNumber));
        }

        Value = value;
    }

    private string Value { get; }

    public static TelephoneNumber New(string value) => new(value);

    public static TelephoneNumber FromString(string telephoneNumber) => new(telephoneNumber);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
