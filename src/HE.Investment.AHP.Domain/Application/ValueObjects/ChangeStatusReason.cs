using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ChangeStatusReason : LongText
{
    public ChangeStatusReason(string value, string fieldDisplayName = "reason")
        : base(value, nameof(ChangeStatusReason), fieldDisplayName)
    {
    }
}
