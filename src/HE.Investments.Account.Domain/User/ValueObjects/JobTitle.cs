using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class JobTitle : RequiredStringValueObject
{
    public JobTitle(string? value)
        : base(value, nameof(JobTitle), "your job title", MaximumInputLength.ShortInput)
    {
    }
}
