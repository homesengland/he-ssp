using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class EmailAddress : ShortText
{
    public EmailAddress(string? value)
        : base(value?.Trim(), nameof(EmailAddress), "email address")
    {
        if (!Regex.IsMatch(Value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
        {
            OperationResult.New()
                .AddValidationError(nameof(EmailAddress), "Enter an email address in the correct format, like name@example.com")
                .CheckErrors();
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value.ToLowerInvariant();
    }
}
