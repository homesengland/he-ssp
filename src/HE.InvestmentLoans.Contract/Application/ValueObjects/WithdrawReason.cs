using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Application.ValueObjects;
public class WithdrawReason : ValueObject
{
    public WithdrawReason(string? value)
    {
        if (value.IsNotProvided())
        {
            OperationResult
            .New()
            .AddValidationError(nameof(WithdrawReason), ValidationErrorMessage.EnterWhyYouWantToWithdrawApplication)
            .CheckErrors();
        }
        else if (value!.Length >= MaximumInputLength.LongInput)
        {
            OperationResult
            .New()
            .AddValidationError(nameof(WithdrawReason), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.WithdrawReason))
            .CheckErrors();
        }

        Value = value!;
    }

    public string Value { get; }

    public static WithdrawReason New(string? value) => new(value);

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
