using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class SecondaryTelephoneNumber : RequiredStringValueObject
{
    public SecondaryTelephoneNumber(string? value)
        : base(value, nameof(SecondaryTelephoneNumber), "your secondary telephone number", MaximumInputLength.ShortInput)
    {
    }
}
