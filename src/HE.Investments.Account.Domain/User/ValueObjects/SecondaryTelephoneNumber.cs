using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class SecondaryTelephoneNumber : RequiredStringValueObject
{
    public SecondaryTelephoneNumber(string? value)
        : base(value, nameof(SecondaryTelephoneNumber), "your secondary telephone number", MaximumInputLength.ShortInput)
    {
    }
}
