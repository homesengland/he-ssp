using HE.Investment.AHP.Domain.Application.Constants;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class RequestToEditReason : LongText
{
    public RequestToEditReason(string? value)
        : base(
            value,
            nameof(RequestToEditReason),
            ApplicationValidationErrors.EnterChangeStatusReason("want to request to edit"),
            ValidationErrorMessage.LongInputLengthExceeded("request to edit"))
    {
    }
}
