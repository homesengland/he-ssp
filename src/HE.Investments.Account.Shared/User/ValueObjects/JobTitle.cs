using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class JobTitle : RequiredStringValueObject
{
    public JobTitle(string? value)
        : base(value, nameof(JobTitle), "your job title", MaximumInputLength.ShortInput)
    {
    }
}
