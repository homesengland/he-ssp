using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class YourJobTitle : YourShortText
{
    public YourJobTitle(string? value)
        : base(value, "JobTitle", "job title")
    {
    }
}
