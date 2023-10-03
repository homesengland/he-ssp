using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.User.ValueObjects;

public class JobTitle : ValueObject
{
    private JobTitle(string value)
    {
        if (value!.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(UserDetailsViewModel.JobTitle), ValidationErrorMessage.EnterJobTitle)
                .CheckErrors();
        }
        else if (value!.Length > MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(
                    nameof(UserDetailsViewModel.JobTitle),
                    ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.JobTitle))
                .CheckErrors();
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
