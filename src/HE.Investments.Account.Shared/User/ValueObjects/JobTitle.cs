using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class JobTitle : ShortText
{
    public JobTitle(string? value)
        : base(value, nameof(JobTitle), "job title")
    {
    }
}
