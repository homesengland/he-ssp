using HE.Investment.AHP.Domain.Application.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class HoldReason : LongText
{
    public HoldReason(string? value)
        : base(
            value,
            nameof(HoldReason),
            ApplicationValidationErrors.EnterHoldReason,
            ValidationErrorMessage.LongInputLengthExceeded("hold reason"))
    {
    }
}
