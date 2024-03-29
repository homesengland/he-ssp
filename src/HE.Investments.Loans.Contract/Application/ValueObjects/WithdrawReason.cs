using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.Application.ValueObjects;
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
        else if (value!.Length > MaximumInputLength.LongInput)
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
