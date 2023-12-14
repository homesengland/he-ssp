using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class JobTitle : RequiredStringValueObject
{
    public JobTitle(string? value, string displayName = "your job title")
        : base(value, nameof(JobTitle), displayName, MaximumInputLength.ShortInput)
    {
    }
}
