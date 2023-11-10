using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class TelephoneNumber : RequiredStringValueObject
{
    public TelephoneNumber(string? value)
        : base(value, nameof(TelephoneNumber), "your preferred telephone number", MaximumInputLength.ShortInput)
    {
    }
}
