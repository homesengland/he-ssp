using HE.Investment.AHP.Domain.Application.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class WithdrawReason : LongText
{
    public WithdrawReason(string? value)
        : base(
            value,
            nameof(WithdrawReason),
            ApplicationValidationErrors.EnterChangeStatusReason("are withdrawing"),
            ValidationErrorMessage.LongInputLengthExceeded("withdraw reason"))
    {
    }
}
