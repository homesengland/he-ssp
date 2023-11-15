using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class TelephoneNumber : RequiredStringValueObject
{
    public TelephoneNumber(string? value)
        : base(value, nameof(TelephoneNumber), "your preferred telephone number", MaximumInputLength.ShortInput)
    {
    }
}
