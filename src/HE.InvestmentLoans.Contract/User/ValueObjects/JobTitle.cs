using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class JobTitle : ValueObjectWithErrorItem
{
    private JobTitle(string value)
    {
        if (value!.IsNotProvided())
        {
            AddValidationError(nameof(UserDetailsViewModel.JobTitle), ValidationErrorMessage.EnterJobTitle);
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            AddValidationError(
                nameof(UserDetailsViewModel.JobTitle),
                ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.JobTitle));
        }

        Value = value;
    }

    private string Value { get; }

    public static JobTitle New(string value) => new(value);

    public static JobTitle FromString(string jobTitle) => new(jobTitle);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
