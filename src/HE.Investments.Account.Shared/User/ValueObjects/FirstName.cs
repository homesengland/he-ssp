using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class FirstName : RequiredStringValueObject
{
    public FirstName(string? value)
        : base(value, nameof(FirstName), "your first name", MaximumInputLength.ShortInput)
    {
    }
}
