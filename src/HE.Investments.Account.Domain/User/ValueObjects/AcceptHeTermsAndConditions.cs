using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Domain.User.ValueObjects;

public class AcceptHeTermsAndConditions : ValueObject
{
    private AcceptHeTermsAndConditions(bool value)
    {
        if (!value)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(AcceptHeTermsAndConditions), ValidationErrorMessage.AcceptTermsAndConditions)
                .CheckErrors();
        }

        Value = value;
    }

    public bool Value { get; }

    public static AcceptHeTermsAndConditions FromString(string? value)
    {
        return value == "checked" ? new AcceptHeTermsAndConditions(true) : new AcceptHeTermsAndConditions(false);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
