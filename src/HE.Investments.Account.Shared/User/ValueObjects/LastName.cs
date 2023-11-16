using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class LastName : RequiredStringValueObject
{
    public LastName(string? value)
        : base(value, nameof(LastName), "your last name", MaximumInputLength.ShortInput)
    {
    }
}
