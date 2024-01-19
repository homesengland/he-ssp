using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class HoldReason : LongText
{
    public HoldReason(string value, string fieldDisplayName = "hold reason")
        : base(value, nameof(WithdrawReason), fieldDisplayName)
    {
    }
}
