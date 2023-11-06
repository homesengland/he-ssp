using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class LastName : RequiredStringValueObject
{
    public LastName(string? value)
        : base(value, nameof(LastName), "your last name", MaximumInputLength.ShortInput)
    {
    }
}
