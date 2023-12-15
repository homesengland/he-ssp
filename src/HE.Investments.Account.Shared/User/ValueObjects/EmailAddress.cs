using System.Text.RegularExpressions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class EmailAddress : RequiredStringValueObject
{
    public EmailAddress(string? value)
        : base(value?.Trim(), nameof(EmailAddress), "email address", MaximumInputLength.ShortInput)
    {
        if (!Regex.IsMatch(Value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
        {
            OperationResult.New()
                .AddValidationError(nameof(EmailAddress), "Email address is not valid")
                .CheckErrors();
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value.ToLowerInvariant();
    }
}
