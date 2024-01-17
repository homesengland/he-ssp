using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class WithdrawReason : LongText
{
    public WithdrawReason(string? value)
        : base(
            value,
            nameof(WithdrawReason),
            ValidationErrorMessage.EnterWithdrawReason,
            ValidationErrorMessage.LongInputLengthExceeded("withdraw reason"))
    {
    }
}
